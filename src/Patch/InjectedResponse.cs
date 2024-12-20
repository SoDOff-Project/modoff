using System;
using UnityEngine.Networking;
using UnityEngine;

namespace modoff.Patch {
    public class InjectedResponse : UtIWWWAsync {
        public InjectedResponse() { }
        public InjectedResponse(string str) {
            data = str;
        }


        public UnityWebRequest pWebRequest => throw new NotImplementedException();

        public string pURL => throw new NotImplementedException();

        public RsResourceType pResourcetype => throw new NotImplementedException();

        public bool pIsDone => throw new NotImplementedException();

        public float pProgress => 50;

        public string pError => throw new NotImplementedException();

        public string pData => data;

        public string url;

        public byte[] pBytes => throw new NotImplementedException();

        public Texture pTexture => throw new NotImplementedException();

        public UnityEngine.AudioClip pAudioClip => throw new NotImplementedException();

        public AssetBundle pAssetBundle => throw new NotImplementedException();

        public bool pFromCache => throw new NotImplementedException();

        private string data;

        public void Download(string inURL, RsResourceType inType, UtWWWEventHandler inCallback, bool inSendProgressEvents, bool inDisableCache, bool inDownLoadOnly, bool inIgnoreAssetVersion) {
            throw new NotImplementedException();
        }

        public void DownloadBundle(string url, Hash128 hash, UtWWWEventHandler callback, bool sendProgressEvents, bool disableCache, bool downloadOnly) {
            throw new NotImplementedException();
        }

        public void Kill() {
            throw new NotImplementedException();
        }

        public void OnSceneLoaded(string inLevel) {
            throw new NotImplementedException();
        }

        public void PostForm(string inURL, WWWForm inForm, UtWWWEventHandler inCallback, bool inSendProgressEvents) {
            throw new NotImplementedException();
        }

        public bool Update() {
            throw new NotImplementedException();
        }

        public void SetData(string data) {
            this.data = data;
        }
    }
}
