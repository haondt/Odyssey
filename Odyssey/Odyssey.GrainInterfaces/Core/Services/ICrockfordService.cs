namespace Odyssey.GrainInterfaces.Core.Services
{
    public interface ICrockfordService
    {
        byte[] Decode(string input);
        string Encode(byte[] value);
        string Normalize(string input);
        string Random(int length);
    }
}
