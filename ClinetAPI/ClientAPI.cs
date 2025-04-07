

namespace ClinetAPI
{
    [Serializable]
    public abstract class ServerCommand
    {
        public string Header;

        protected ServerCommand(string header)
        {
            Header = header;
        }
    }


}
