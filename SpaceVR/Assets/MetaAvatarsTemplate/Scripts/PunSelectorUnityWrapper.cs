using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Oculus.Interaction;
using UnityEngine.Events;
using UnityEngine.Assertions;
public class PunSelectorUnityWrapper : MonoBehaviour
{
    [SerializeField, Interface(typeof(ISelector))]
    private MonoBehaviour _selector;
    private ISelector Selector;

    [SerializeField]
    private UnityEvent _whenSelected;

    [SerializeField]
    private UnityEvent _whenUnselected;

    public UnityEvent WhenSelected => _whenSelected;
    public UnityEvent WhenUnselected => _whenUnselected;

    [SerializeField]
    PhotonView _view;

    protected bool _started = false;

    protected virtual void Awake()
    {
        Selector = _selector as ISelector;
    }

    protected virtual void Start()
    {
        this.BeginStart(ref _started);
        Assert.IsNotNull(Selector);
        this.EndStart(ref _started);
    }

    
    protected virtual void OnEnable()
    {
        if (_started)
        {
            Selector.WhenSelected += HandleSelected;
            Selector.WhenUnselected += HandleUnselected;
        }
    }

    
    protected virtual void OnDisable()
    {
        if (_started)
        {
            Selector.WhenSelected -= HandleSelected;
            Selector.WhenUnselected -= HandleUnselected;
        }
    }

    private void HandleSelected()
    {
        _view.RPC("HandSelectedInvoke", RpcTarget.All);
    }

    private void HandleUnselected()
    {
        _view.RPC("HandUnSelectedInvoke", RpcTarget.All);
    }
    [PunRPC]
    private void HandSelectedInvoke()
    {
        _whenSelected.Invoke();
    }
    [PunRPC]
    private void HandUnSelectedInvoke()
    {
        _whenUnselected.Invoke();
    }

    #region Inject

    public void InjectAllSelectorUnityEventWrapper(ISelector selector)
    {
        InjectSelector(selector);
    }

    public void InjectSelector(ISelector selector)
    {
        _selector = selector as MonoBehaviour;
        Selector = selector;
    }

    #endregion
}
