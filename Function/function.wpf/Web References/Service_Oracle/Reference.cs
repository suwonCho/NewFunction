﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 이 소스 코드가 Microsoft.VSDesigner, 버전 4.0.30319.42000에서 자동으로 생성되었습니다.
// 
#pragma warning disable 1591

namespace function.wpf.Service_Oracle {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Service_OracleSoap", Namespace="http://tempuri.org/")]
    public partial class Service_Oracle : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback param_sperator_getOperationCompleted;
        
        private System.Threading.SendOrPostCallback item_sperator_getOperationCompleted;
        
        private System.Threading.SendOrPostCallback Excute_Table_GetOperationCompleted;
        
        private System.Threading.SendOrPostCallback Excute_TableOperationCompleted;
        
        private System.Threading.SendOrPostCallback Excute_Table_RtnOperationCompleted;
        
        private System.Threading.SendOrPostCallback Excute_QueryOperationCompleted;
        
        private System.Threading.SendOrPostCallback Excute_Query_CntOperationCompleted;
        
        private System.Threading.SendOrPostCallback Update_FileInfoGetOperationCompleted;
        
        private System.Threading.SendOrPostCallback Update_FileStreamGetOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service_Oracle() {
            this.Url = global::function.wpf.Properties.Settings.Default.function_wpf_Service_Oracle_Service_Oracle;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event param_sperator_getCompletedEventHandler param_sperator_getCompleted;
        
        /// <remarks/>
        public event item_sperator_getCompletedEventHandler item_sperator_getCompleted;
        
        /// <remarks/>
        public event Excute_Table_GetCompletedEventHandler Excute_Table_GetCompleted;
        
        /// <remarks/>
        public event Excute_TableCompletedEventHandler Excute_TableCompleted;
        
        /// <remarks/>
        public event Excute_Table_RtnCompletedEventHandler Excute_Table_RtnCompleted;
        
        /// <remarks/>
        public event Excute_QueryCompletedEventHandler Excute_QueryCompleted;
        
        /// <remarks/>
        public event Excute_Query_CntCompletedEventHandler Excute_Query_CntCompleted;
        
        /// <remarks/>
        public event Update_FileInfoGetCompletedEventHandler Update_FileInfoGetCompleted;
        
        /// <remarks/>
        public event Update_FileStreamGetCompletedEventHandler Update_FileStreamGetCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld(string pass) {
            object[] results = this.Invoke("HelloWorld", new object[] {
                        pass});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync(string pass) {
            this.HelloWorldAsync(pass, null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(string pass, object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[] {
                        pass}, this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/param_sperator_get", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string param_sperator_get() {
            object[] results = this.Invoke("param_sperator_get", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void param_sperator_getAsync() {
            this.param_sperator_getAsync(null);
        }
        
        /// <remarks/>
        public void param_sperator_getAsync(object userState) {
            if ((this.param_sperator_getOperationCompleted == null)) {
                this.param_sperator_getOperationCompleted = new System.Threading.SendOrPostCallback(this.Onparam_sperator_getOperationCompleted);
            }
            this.InvokeAsync("param_sperator_get", new object[0], this.param_sperator_getOperationCompleted, userState);
        }
        
        private void Onparam_sperator_getOperationCompleted(object arg) {
            if ((this.param_sperator_getCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.param_sperator_getCompleted(this, new param_sperator_getCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/item_sperator_get", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string item_sperator_get() {
            object[] results = this.Invoke("item_sperator_get", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void item_sperator_getAsync() {
            this.item_sperator_getAsync(null);
        }
        
        /// <remarks/>
        public void item_sperator_getAsync(object userState) {
            if ((this.item_sperator_getOperationCompleted == null)) {
                this.item_sperator_getOperationCompleted = new System.Threading.SendOrPostCallback(this.Onitem_sperator_getOperationCompleted);
            }
            this.InvokeAsync("item_sperator_get", new object[0], this.item_sperator_getOperationCompleted, userState);
        }
        
        private void Onitem_sperator_getOperationCompleted(object arg) {
            if ((this.item_sperator_getCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.item_sperator_getCompleted(this, new item_sperator_getCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Excute_Table_Get", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable Excute_Table_Get() {
            object[] results = this.Invoke("Excute_Table_Get", new object[0]);
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public void Excute_Table_GetAsync() {
            this.Excute_Table_GetAsync(null);
        }
        
        /// <remarks/>
        public void Excute_Table_GetAsync(object userState) {
            if ((this.Excute_Table_GetOperationCompleted == null)) {
                this.Excute_Table_GetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExcute_Table_GetOperationCompleted);
            }
            this.InvokeAsync("Excute_Table_Get", new object[0], this.Excute_Table_GetOperationCompleted, userState);
        }
        
        private void OnExcute_Table_GetOperationCompleted(object arg) {
            if ((this.Excute_Table_GetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Excute_Table_GetCompleted(this, new Excute_Table_GetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Excute_Table", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Excute_Table(string pass, System.Data.DataTable dt) {
            object[] results = this.Invoke("Excute_Table", new object[] {
                        pass,
                        dt});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void Excute_TableAsync(string pass, System.Data.DataTable dt) {
            this.Excute_TableAsync(pass, dt, null);
        }
        
        /// <remarks/>
        public void Excute_TableAsync(string pass, System.Data.DataTable dt, object userState) {
            if ((this.Excute_TableOperationCompleted == null)) {
                this.Excute_TableOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExcute_TableOperationCompleted);
            }
            this.InvokeAsync("Excute_Table", new object[] {
                        pass,
                        dt}, this.Excute_TableOperationCompleted, userState);
        }
        
        private void OnExcute_TableOperationCompleted(object arg) {
            if ((this.Excute_TableCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Excute_TableCompleted(this, new Excute_TableCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Excute_Table_Rtn", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet Excute_Table_Rtn(string pass, System.Data.DataTable dt) {
            object[] results = this.Invoke("Excute_Table_Rtn", new object[] {
                        pass,
                        dt});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void Excute_Table_RtnAsync(string pass, System.Data.DataTable dt) {
            this.Excute_Table_RtnAsync(pass, dt, null);
        }
        
        /// <remarks/>
        public void Excute_Table_RtnAsync(string pass, System.Data.DataTable dt, object userState) {
            if ((this.Excute_Table_RtnOperationCompleted == null)) {
                this.Excute_Table_RtnOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExcute_Table_RtnOperationCompleted);
            }
            this.InvokeAsync("Excute_Table_Rtn", new object[] {
                        pass,
                        dt}, this.Excute_Table_RtnOperationCompleted, userState);
        }
        
        private void OnExcute_Table_RtnOperationCompleted(object arg) {
            if ((this.Excute_Table_RtnCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Excute_Table_RtnCompleted(this, new Excute_Table_RtnCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Excute_Query", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet Excute_Query(string pass, string query) {
            object[] results = this.Invoke("Excute_Query", new object[] {
                        pass,
                        query});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void Excute_QueryAsync(string pass, string query) {
            this.Excute_QueryAsync(pass, query, null);
        }
        
        /// <remarks/>
        public void Excute_QueryAsync(string pass, string query, object userState) {
            if ((this.Excute_QueryOperationCompleted == null)) {
                this.Excute_QueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExcute_QueryOperationCompleted);
            }
            this.InvokeAsync("Excute_Query", new object[] {
                        pass,
                        query}, this.Excute_QueryOperationCompleted, userState);
        }
        
        private void OnExcute_QueryOperationCompleted(object arg) {
            if ((this.Excute_QueryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Excute_QueryCompleted(this, new Excute_QueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Excute_Query_Cnt", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Excute_Query_Cnt(string pass, string query, int cnt) {
            object[] results = this.Invoke("Excute_Query_Cnt", new object[] {
                        pass,
                        query,
                        cnt});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void Excute_Query_CntAsync(string pass, string query, int cnt) {
            this.Excute_Query_CntAsync(pass, query, cnt, null);
        }
        
        /// <remarks/>
        public void Excute_Query_CntAsync(string pass, string query, int cnt, object userState) {
            if ((this.Excute_Query_CntOperationCompleted == null)) {
                this.Excute_Query_CntOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExcute_Query_CntOperationCompleted);
            }
            this.InvokeAsync("Excute_Query_Cnt", new object[] {
                        pass,
                        query,
                        cnt}, this.Excute_Query_CntOperationCompleted, userState);
        }
        
        private void OnExcute_Query_CntOperationCompleted(object arg) {
            if ((this.Excute_Query_CntCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Excute_Query_CntCompleted(this, new Excute_Query_CntCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Update_FileInfoGet", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable Update_FileInfoGet() {
            object[] results = this.Invoke("Update_FileInfoGet", new object[0]);
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public void Update_FileInfoGetAsync() {
            this.Update_FileInfoGetAsync(null);
        }
        
        /// <remarks/>
        public void Update_FileInfoGetAsync(object userState) {
            if ((this.Update_FileInfoGetOperationCompleted == null)) {
                this.Update_FileInfoGetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdate_FileInfoGetOperationCompleted);
            }
            this.InvokeAsync("Update_FileInfoGet", new object[0], this.Update_FileInfoGetOperationCompleted, userState);
        }
        
        private void OnUpdate_FileInfoGetOperationCompleted(object arg) {
            if ((this.Update_FileInfoGetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Update_FileInfoGetCompleted(this, new Update_FileInfoGetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Update_FileStreamGet", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Update_FileStreamGet(string filename) {
            object[] results = this.Invoke("Update_FileStreamGet", new object[] {
                        filename});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void Update_FileStreamGetAsync(string filename) {
            this.Update_FileStreamGetAsync(filename, null);
        }
        
        /// <remarks/>
        public void Update_FileStreamGetAsync(string filename, object userState) {
            if ((this.Update_FileStreamGetOperationCompleted == null)) {
                this.Update_FileStreamGetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdate_FileStreamGetOperationCompleted);
            }
            this.InvokeAsync("Update_FileStreamGet", new object[] {
                        filename}, this.Update_FileStreamGetOperationCompleted, userState);
        }
        
        private void OnUpdate_FileStreamGetOperationCompleted(object arg) {
            if ((this.Update_FileStreamGetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Update_FileStreamGetCompleted(this, new Update_FileStreamGetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void param_sperator_getCompletedEventHandler(object sender, param_sperator_getCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class param_sperator_getCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal param_sperator_getCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void item_sperator_getCompletedEventHandler(object sender, item_sperator_getCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class item_sperator_getCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal item_sperator_getCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void Excute_Table_GetCompletedEventHandler(object sender, Excute_Table_GetCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Excute_Table_GetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Excute_Table_GetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataTable Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataTable)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void Excute_TableCompletedEventHandler(object sender, Excute_TableCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Excute_TableCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Excute_TableCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void Excute_Table_RtnCompletedEventHandler(object sender, Excute_Table_RtnCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Excute_Table_RtnCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Excute_Table_RtnCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void Excute_QueryCompletedEventHandler(object sender, Excute_QueryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Excute_QueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Excute_QueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void Excute_Query_CntCompletedEventHandler(object sender, Excute_Query_CntCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Excute_Query_CntCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Excute_Query_CntCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void Update_FileInfoGetCompletedEventHandler(object sender, Update_FileInfoGetCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Update_FileInfoGetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Update_FileInfoGetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataTable Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataTable)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void Update_FileStreamGetCompletedEventHandler(object sender, Update_FileStreamGetCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Update_FileStreamGetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Update_FileStreamGetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591