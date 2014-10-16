﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWFormatLib.DBC;
using WoWFormatLib.Utils;
namespace WoWFormatLib.DBC
{
    public class DBCReader<T>
    {
        public DBCHeader header;
        public T[] records;
        public Dictionary<int, string> stringblock;

        public DBCReader()
        {
        }

        public void LoadDBC(string filename)
        {
            if(!CASC.IsCASCInit)
                CASC.InitCasc();

            if (!CASC.FileExists(filename))
            {
                new MissingFile(filename);
                return; //well shit what now
            }

            using (BinaryReader bin = new BinaryReader(File.Open(Path.Combine("data", filename), FileMode.Open)))
            {
                header = bin.Read<DBCHeader>();

                records = new T[header.record_count];

                for (int i = 0; i < header.record_count; i++)
                {
                    records[i] = bin.Read<T>();
                }

                int stringblock_start = (int)bin.BaseStream.Position;

                stringblock = new Dictionary<int, string>();

                while (bin.BaseStream.Position != bin.BaseStream.Length)
                {
                    int index = (int)bin.BaseStream.Position - stringblock_start;
                    stringblock[index] = bin.ReadStringNull();
                }
            }
        }
    }
}
