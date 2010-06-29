using System;
using System.Collections.Generic;
using System.Text;

namespace NUS_Downloader
{
    class TMD
    {
        // TMD Variables
        //public byte[] SignatureType;
 



        /* Signature Types
        public enum SigTypes : byte[]
        {
            RSA_2048 = new byte[4] {0x00, 0x01, 0x00, 0x01},
            RSA_4048 = new byte[4] {0x00, 0x01, 0x00, 0x01}
        } */
        /*
         * Title metadata is a format used to store information about a title (a single standalone game, channel, etc.) and all its installed contents, including which contents they consist of and their SHA1 hashes.

Many operations are done in terms of 64-byte blocks, which means you will often see padding out to the nearest 64-byte boundary at the end of a field.
Contents

Structure
Header
Start 	Length 	Description
0x000 	4 	Signature type
0x004 	256 	Signature
0x104 	60 	Padding modulo 64
0x140 	64 	Issuer
0x180 	1 	Version
0x181 	1 	ca_crl_version
0x182 	1 	signer_crl_version
0x183 	1 	Padding modulo 64
0x184 	8 	System Version (the ios that the title need)
0x18C 	8 	Title ID
0x194 	4 	Title type
0x198 	2 	Group ID
0x19A 	62 	reserved
0x1D8 	4 	Access rights (flags for DVD-video access and full PPC hardware access)
0x1DC 	2 	Title version
0x1DE 	2 	Number of contents (nbr_cont)
0x1E0 	2 	boot index
0x1E2 	2 	Padding modulo 64
0x1E4 	36*nbr_cont 	Contents
Content
Start 	Length 	Description
0x00 	4 	Content ID
0x04 	2 	Index
0x06 	2 	Type
0x08 	8 	Size
0x10 	20 	SHA1 hash
Certificates
Start 	Length 	Description
0x000 	4 	Signature type
0x004 	256 	Signature
0x104 	32 	Issuer
0x124 	4 	Tag
0x128 	64 	Name
0x168 		Key
*/


    }
}
