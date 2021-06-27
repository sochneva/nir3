//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.1
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class Configuration : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal Configuration(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Configuration obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~Configuration() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          CoolPropPINVOKE.delete_Configuration(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public Configuration() : this(CoolPropPINVOKE.new_Configuration(), true) {
    if (CoolPropPINVOKE.SWIGPendingException.Pending) throw CoolPropPINVOKE.SWIGPendingException.Retrieve();
  }

  public ConfigurationItem get_item(configuration_keys key) {
    ConfigurationItem ret = new ConfigurationItem(CoolPropPINVOKE.Configuration_get_item(swigCPtr, (int)key), false);
    if (CoolPropPINVOKE.SWIGPendingException.Pending) throw CoolPropPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void add_item(ConfigurationItem item) {
    CoolPropPINVOKE.Configuration_add_item(swigCPtr, ConfigurationItem.getCPtr(item));
    if (CoolPropPINVOKE.SWIGPendingException.Pending) throw CoolPropPINVOKE.SWIGPendingException.Retrieve();
  }

  public SWIGTYPE_p_std__mapT_configuration_keys_CoolProp__ConfigurationItem_t get_items() {
    SWIGTYPE_p_std__mapT_configuration_keys_CoolProp__ConfigurationItem_t ret = new SWIGTYPE_p_std__mapT_configuration_keys_CoolProp__ConfigurationItem_t(CoolPropPINVOKE.Configuration_get_items(swigCPtr), false);
    if (CoolPropPINVOKE.SWIGPendingException.Pending) throw CoolPropPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void set_defaults() {
    CoolPropPINVOKE.Configuration_set_defaults(swigCPtr);
    if (CoolPropPINVOKE.SWIGPendingException.Pending) throw CoolPropPINVOKE.SWIGPendingException.Retrieve();
  }

}
