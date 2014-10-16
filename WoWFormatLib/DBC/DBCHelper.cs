﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWFormatLib.DBC
{
    public class DBCHelper
    {
        public static string[] getTexturesByModelFilename(string modelfilename, int flag)
        {
            List<string> filenames = new List<string>();
            if (flag == 1 || flag == 11)
            {
                DBCReader<FileDataRecord> reader = new DBCReader<FileDataRecord>();
                reader.LoadDBC("DBFilesClient\\FileData.dbc");
                for (int i = 0; i < reader.records.Count(); i++)
                {
                    if (DBCHelper.getString(reader.records[i].FileName, reader.stringblock) == modelfilename + ".M2")
                    {
                        Console.WriteLine("Found ID in FileData.dbc: " + reader.records[i].ID);
                        DBCReader<CreatureModelDataRecord> cmdreader = new DBCReader<CreatureModelDataRecord>();
                        cmdreader.LoadDBC("DBFilesClient\\CreatureModelData.dbc");
                        for (int cmdi = 0; cmdi < cmdreader.records.Count(); cmdi++)
                        {
                            if (reader.records[i].ID == cmdreader.records[cmdi].fileDataID)
                            {
                                Console.WriteLine("Found Creature ID in CreatureModelData.dbc: " + cmdreader.records[cmdi].ID);
                                DBCReader<CreatureDisplayInfoRecord> cdireader = new DBCReader<CreatureDisplayInfoRecord>();
                                cdireader.LoadDBC("DBFilesClient\\CreatureDisplayInfo.dbc");
                                for (int cdii = 0; cdii < cdireader.records.Count(); cdii++)
                                {
                                    if (cdireader.records[cdii].modelID == cmdreader.records[cmdi].ID)
                                    {
                                        filenames.Add(getString(cdireader.records[cdii].textureVariation_0, cdireader.stringblock));
                                    }
                                }
                            }
                        }
                    }
                }
            }
           
            string[] ret = filenames.Distinct().ToArray();
            return ret;
        }

        public static string getString(uint offset, Dictionary<int, string> stringblock)
        {
            Console.WriteLine("Using old getstring method, please refer to dictionary");
            return stringblock[(int)offset];
        }
    }
}
