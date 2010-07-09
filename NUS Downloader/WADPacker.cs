///////////////////////////////////////////
// NUS Downloader: WADPacker.cs          //
// $Rev::                              $ //
// $Author::                           $ //
// $Date::                             $ //
///////////////////////////////////////////

///////////////////////////////////////
// Copyright (C) 2010
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>
///////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NUS_Downloader
{
    /// <summary>
    /// Class for handling WAD Packaging.
    /// </summary>
    class WADPacker
    {
        // WAD Component Variables
        private byte[] Certsys;
        public byte[] Certs { get { return Certsys;} set { Certsys = value; CertChainSize = Certsys.Length; } }
        private byte[] tmd;
        public byte[] TMD { get { return tmd; } 
            set 
            {
                tmd = value;
                TMDContentCount = ContentCount(TMD);
                TMDSize = 484 + (TMDContentCount * 36);
            } }
        public byte[] Ticket;
        private int TMDContentCount;

        // WAD Contents
        private byte[][] TMDContents;
        public byte[][] Contents { get { return TMDContents; } 
            set {
                TMDContents = value;
                for (int a = 0; a < TMDContents.Length; a++)
                {
                    DataSize += TMDContents[a].Length;
                }
            } }
        
        // WAD Saving Variables
        public string Directory;
        public string FileName;

        // TMD Informations
        public string[] tmdnames;
        public string[] tmdsizes;

        // WAD Header Variables
        private const int HeaderSize = 0x20;
        private int CertChainSize;
        private const int TicketSize = 0x2A4;
        private int TMDSize;
        private int DataSize;
        private byte[] WADMagic = new byte[8] { 0x00, 0x00, 0x00, 0x20, 0x49, 0x73, 0x00, 0x00 };
        private byte[] RESERVED_CONST = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        private byte[] TIKSIZE_CONST = new byte[4] { 0x00, 0x00, 0x02, 0xA4 };

        // Report Status back in EventHandler
        public delegate void StatusChangedEventHandler(string status);
        public event StatusChangedEventHandler StatusChanged;

     
        /// <summary>
        /// Pads byte[].
        /// </summary>
        /// <param name="src">The byte[] or binary to be padded.</param>
        /// <param name="pad">How much to pad by.</param>
        /// <returns>Padded byte[]</returns>
        private long PadToMultipleOf(long src, int pad)
        {
            long len = (src + pad - 1) / pad * pad;
            return len;
        }

        /// <summary>
        /// Converts an integer into its equivilant byte array.
        /// </summary>
        /// <param name="theInt">The integer</param>
        /// <param name="arrayLen">Length you desire the byte[] to be.</param>
        /// <returns></returns>
        private byte[] ConvertInttoByteArray(int theInt, int arrayLen)
        {
            byte[] resultArray = new byte[arrayLen];
            for (int i = arrayLen - 1; i >= 0; i--)
            {
                resultArray[i] = (byte)((theInt >> (8 * i)) & 0xFF);
            }
            Array.Reverse(resultArray);

            // Fix duplication, rewrite extra to 0x00;
            if (arrayLen > 4)
            {
                for (int i = 0; i < (arrayLen - 4); i++)
                    resultArray[i] = 0x00;
            }
            return resultArray;
        }

        /// <summary>
        /// Handles the size mismatch.
        /// </summary>
        /// <param name="contentsize">The contentsize.</param>
        /// <param name="actualsize">The actualsize.</param>
        void HandleMismatch(int contentsize, int actualsize)
        {
            if (contentsize != actualsize)
                if ((contentsize - actualsize) > 16)
                    StatusChanged(String.Format(" (BAD Mismatch) (Dif: {0}", (contentsize - actualsize)));
                //else
                    //statusbox.Text += " (Safe Mismatch)";
        }

        /// <summary>
        /// Returns content count of TMD
        /// </summary>
        /// <param name="tmd">The TMD.</param>
        /// <returns>int Count of Contents</returns>
        private int ContentCount(byte[] tmd)
        { 
            return (tmd[0x1DE] * 256) + tmd[0x1DF];
        }

        /// <summary>
        /// Packs the WAD file, saves it to specified location.
        /// </summary>
        public void PackWAD()
        {
            if ((String.IsNullOrEmpty(Directory)) || (String.IsNullOrEmpty(FileName)))
            {
                StatusChanged("ERROR: No Directory/FileName provided!");
                return;
            }

            FileStream wadfs = new FileStream(Path.Combine(Directory, FileName), FileMode.Create);

            // Seek the beginning of the WAD...
            wadfs.Seek(0, SeekOrigin.Begin);

            // Write initial part of header (WADType)
            wadfs.Write(WADMagic, 0, WADMagic.Length);

            // Write CertChainLength
            wadfs.Seek(0x08, SeekOrigin.Begin);
            byte[] chainsize = ConvertInttoByteArray(CertChainSize, 4);
            wadfs.Write(chainsize, 0, chainsize.Length);

            // Write res
            wadfs.Seek(0x0C, SeekOrigin.Begin);
            wadfs.Write(RESERVED_CONST, 0, RESERVED_CONST.Length);

            // Write ticketsize
            wadfs.Seek(0x10, SeekOrigin.Begin);
            wadfs.Write(TIKSIZE_CONST, 0, TIKSIZE_CONST.Length);

            // Write tmdsize
            wadfs.Seek(0x14, SeekOrigin.Begin);
            byte[] tmdsize = ConvertInttoByteArray(TMDSize, 4);
            wadfs.Write(tmdsize, 0, tmdsize.Length);

            // Write data size
            wadfs.Seek(0x18, SeekOrigin.Begin);
            wadfs.Write(ConvertInttoByteArray(DataSize, 4), 0, 4);
            StatusChanged(" - Header wrote (0x00)");

            // Write cert[] to 0x40.
            wadfs.Seek(0x40, SeekOrigin.Begin);
            wadfs.Write(Certsys, 0, Certsys.Length);
            StatusChanged(String.Format(" - Certs wrote (0x{0})", Convert.ToString(64, 16)));

            // Pad to next 64 byte boundary.
            wadfs.Seek(2624, SeekOrigin.Begin);

            // Write ticket at this point...
            wadfs.Write(Ticket, 0, TicketSize);
            StatusChanged(String.Format(" - Ticket wrote (0x{0})", Convert.ToString((wadfs.Length - 0x2A4), 16)));

            // Pad to next 64 byte boundary.
            wadfs.Seek(PadToMultipleOf(wadfs.Length, 64), SeekOrigin.Begin);

            // Write TMD at this point...
            wadfs.Write(tmd, 0, TMDSize);
            StatusChanged(String.Format(" - TMD wrote (0x{0})", Convert.ToString((wadfs.Length - TMDSize), 16)));

            // Add the individual contents
            for (int a = 0; a < TMDContentCount; a++)
            {
                // Pad to next 64 byte boundary...
                wadfs.Seek(PadToMultipleOf(wadfs.Length, 64), SeekOrigin.Begin);

                wadfs.Write(TMDContents[a], 0, Contents[a].Length);

                StatusChanged(String.Format(" - {0} wrote (0x{1})", tmdnames[a], Convert.ToString((wadfs.Length - TMDContents[a].Length), 16)));
                HandleMismatch(int.Parse(tmdsizes[a], System.Globalization.NumberStyles.HexNumber), TMDContents[a].Length);
            }

            // Close filesystem...
            wadfs.Close();

            // Finished.
            StatusChanged("WAD Created: " + FileName);
        }
    }
}
