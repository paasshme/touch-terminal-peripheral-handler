namespace ProjetS4Test
{
    public class StatusObject
    {
        public bool proxyStatus;
        public bool eventHandlerStatus;
        public bool socketHandlerStatus;
        public StatusObject()
        {
            this.proxyStatus = false;
            this.eventHandlerStatus = false;
            this.socketHandlerStatus = false;

        }

        public void setProxyStatus(bool b)
        {
            this.proxyStatus = b;
        }

        public void setEventHandlerStatus(bool b)
        {
            this.eventHandlerStatus = b;
        }

        public void setSocketHandlerStatus(bool b)
        {
            this.socketHandlerStatus = b;
        }
    }
}