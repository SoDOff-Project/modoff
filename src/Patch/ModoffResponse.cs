namespace modoff.Patch {
    public class ModoffResponse {
        public InjectedResponse injectedRes;
        public UtWWWEventHandler inCallback;
        public UtAsyncEvent asyncEvent;
        public ModoffResponse(InjectedResponse res, UtWWWEventHandler inCallback, UtAsyncEvent asyncEvent) {
            injectedRes = res;
            this.inCallback = inCallback;
            this.asyncEvent = asyncEvent;
        }
    }
}
