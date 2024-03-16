//Written for Avem33. https://store.steampowered.com/app/673900/
using System;
using System.IO;

namespace Avem33_Unpacker
{
    class Program
    {
        public static BinaryReader br;

        private static void Main(string[] args)
        {
            br = new BinaryReader(File.OpenRead(args[0]));
            Directory.CreateDirectory(Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]));
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                string name = NullTerminatedString();

                byte readbyte = 0xFE;
                while (readbyte == 0xFE)
                    readbyte = br.ReadByte();//padding
                br.BaseStream.Position--;
                using FileStream FS = File.Create(Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]) + "//" + name);
                BinaryWriter bw = new(FS);
                bw.Write(br.ReadBytes(br.ReadInt32()));
                bw.Close();
            }
        }

        public static string NullTerminatedString()
        {
            char[] fileName = Array.Empty<char>();
            char readchar = (char)1;
            while (readchar > 0)
            {
                readchar = br.ReadChar();
                Array.Resize(ref fileName, fileName.Length + 1);
                fileName[^1] = readchar;
            }
            Array.Resize(ref fileName, fileName.Length - 1);
            string name = new(fileName);
            return name;
        }
    }
}
