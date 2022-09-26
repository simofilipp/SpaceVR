using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Photon.Pun;
using Issimissimo.OVR;



namespace Issimissimo.OVR.HandsInteraction
{
    public class HandGrabbablePun : MonoBehaviour
    {
        [SerializeField] Grabbable grabbable;

        PhotonView _pv;
        Rigidbody _rb;
        bool _isKinematic;


        void Awake()
        {
            _pv = GetComponent<PhotonView>();
            _rb = GetComponent<Rigidbody>();
            _isKinematic = _rb.isKinematic;
        }

        void OnEnable()
        {
            grabbable.OnBeginTransform += OnBeginTransform;
            grabbable.OnEndTransform += OnEndTransform;
        }

        void OnDisable()
        {
            grabbable.OnBeginTransform -= OnBeginTransform;
            grabbable.OnEndTransform -= OnEndTransform;
        }

        void OnBeginTransform()
        {
            //OVRLogger.instance.Log("OnBeginTransform");

            _rb.isKinematic = true;

            _pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            _pv.RPC("RPC_SetKinematic", RpcTarget.All, true); //changes the kinematic state of the object to all players when its grabbed
        }

        void OnEndTransform()
        {
            //OVRLogger.instance.Log("OnEndTransform");

            _rb.isKinematic = _isKinematic;

            _pv.RPC("RPC_SetKinematic", RpcTarget.All, _isKinematic);
        }
        [PunRPC]
        public void RPC_SetKinematic(bool b)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = b;
        }
    }
}
