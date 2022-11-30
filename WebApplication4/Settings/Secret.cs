using System.Text;

namespace WebApplication4.Settings
{
    public static class Secret
    {
        public static string Value
        {
            get
            {
                return "43e4dbf0-52ed-4203-895d-42b586496bd4";
            }
        }
        public static byte[] Bytes
        {
            get
            {
                return Encoding.ASCII.GetBytes(Value);
            }
        }
    }
}
