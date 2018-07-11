using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

// https://stackoverflow.com/questions/187495/how-to-read-assembly-attributes
// copied as of January 19, 2018

namespace Extensions
{

    public static class AssemblyInfoReader
    {

        private static Assembly m_assembly;

        // constructor
        static AssemblyInfoReader()
        {
            m_assembly = Assembly.GetEntryAssembly();
        }


        public static void Configure(Assembly ass)
        {
            m_assembly = ass;
        }


        public static T GetCustomAttribute<T>() where T : Attribute
        {
            object[] customAttributes = m_assembly.GetCustomAttributes(typeof(T), false);
            if (customAttributes.Length != 0)
            {
                return (T)((object)customAttributes[0]);
            }
            return default(T);
        }

        public static string GetCustomAttribute<T>(Func<T, string> getProperty) where T : Attribute
        {
            T customAttribute = GetCustomAttribute<T>();
            if (customAttribute != null)
            {
                return getProperty(customAttribute);
            }
            return null;
        }

        public static int GetCustomAttribute<T>(Func<T, int> getProperty) where T : Attribute
        {
            T customAttribute = GetCustomAttribute<T>();
            if (customAttribute != null)
            {
                return getProperty(customAttribute);
            }
            return 0;
        }



        public static Version Version
        {
            get
            {
                return m_assembly.GetName().Version;
            }
        }

        public static String FullName
        {
            get
            {
                return m_assembly.FullName;
            }
        }

        public static byte[] HashFile(string filename)
        {
            var file = new FileInfo(filename);
            byte[] result = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16 }; // bad value but something of file not found
            using (FileStream stream = file.OpenRead()) // this shares with open files, no crash
            {
                using (var sha = new SHA512Managed())
                {
                    result = sha.ComputeHash(stream);
                }
            }
            return result;
        }

        public static UInt64 HashFile64 (string filename)
        {
            var file = new FileInfo(filename);
            UInt64 result = 0x0102030405060708; // bad value but something of file not found
            using (FileStream stream = file.OpenRead()) // this shares with open files, no crash
            {
                using (var sha = new SHA512Managed())
                {
                    var filehash = sha.ComputeHash(stream);
                    result = BitConverter.ToUInt64(filehash, 0);
                }
            }
            return result;
        }

        public static byte[] PublicKeyToken
        {
            get
            {
                var bytes = m_assembly.GetName().GetPublicKeyToken();
                //var bytes2 = m_assembly.GetName().GetPublicKey();
                if (bytes == null || bytes.Length == 0)
                {
                    return HashFile(m_assembly.Location);
                }
                else
                {
                    return bytes;
                }
            }
        }
        public static byte[] PublicKey
        {
            get
            {
                var bytes = m_assembly.GetName().GetPublicKey();
                if (bytes == null || bytes.Length == 0)
                {
                    return HashFile(m_assembly.Location);
                }
                else
                {
                    return bytes;
                }
            }
        }

        public static string Title
        {
            get
            {
                return GetCustomAttribute<AssemblyTitleAttribute>(
                    delegate (AssemblyTitleAttribute a)
                    {
                        return a.Title;
                    }
                );
            }
        }

        public static string Description
        {
            get
            {
                return GetCustomAttribute<AssemblyDescriptionAttribute>(
                    delegate (AssemblyDescriptionAttribute a)
                    {
                        return a.Description;
                    }
                );
            }
        }


        public static string Product
        {
            get
            {
                return GetCustomAttribute<AssemblyProductAttribute>(
                    delegate (AssemblyProductAttribute a)
                    {
                        return a.Product;
                    }
                );
            }
        }


        public static string Copyright
        {
            get
            {
                return GetCustomAttribute<AssemblyCopyrightAttribute>(
                    delegate (AssemblyCopyrightAttribute a)
                    {
                        return a.Copyright;
                    }
                );
            }
        }



        public static string Company
        {
            get
            {
                return GetCustomAttribute<AssemblyCompanyAttribute>(
                    delegate (AssemblyCompanyAttribute a)
                    {
                        return a.Company;
                    }
                );
            }
        }


        public static string InformationalVersion
        {
            get
            {
                return GetCustomAttribute<AssemblyInformationalVersionAttribute>(
                    delegate (AssemblyInformationalVersionAttribute a)
                    {
                        return a.InformationalVersion;
                    }
                );
            }
        }



        //public static int ProductId
        //{
        //    get
        //    {
        //        return GetCustomAttribute<AssemblyProductIdAttribute>(
        //            delegate (AssemblyProductIdAttribute a)
        //            {
        //                return a.ProductId;
        //            }
        //        );
        //    }
        //}


        public static string Location
        {
            get
            {
                return m_assembly.Location;
            }
        }

        //public static string Location2
        //{
        //    get
        //    {
        //        var ca = m_assembly.CustomAttributes;
        //        //var cb = ca.("x");
        //        return "oops";
        //    }
        //}

        public static string Metadata
        {
            get
            {
                return GetCustomAttribute<AssemblyMetadataAttribute>(
                    delegate (AssemblyMetadataAttribute a)
                    {
                        if (a.Key == "SystemTag")
                        {
                            return a.Value;
                        }
                        else
                        {
                            return "";
                        }
                    }
                );

            }
        }

        //AssemblyMetadataAttribute foo = AssemblyInfo.GetCustomAttribute<AssemblyMetadataAttribute>();
        ////var x = Assembly.GetEntryAssembly().GetCustomAttribute(AssemblyName>
        ////var foo = TaskExtensions::
        //return "";// Extensions. GetCustomAttribute<AssemblyTitleAttribute>(a => a.Title);

        //https://www.codeproject.com/Tips/370232/Where-should-I-store-my-data
        /// <summary>
        /// Get the Application Guid
        /// </summary>
        public static Guid AppGuid
        {
            get
            {
                Assembly asm = Assembly.GetEntryAssembly();
                object[] attr = (asm.GetCustomAttributes(typeof(GuidAttribute), true));
                return new Guid((attr[0] as GuidAttribute).Value);
            }
        }
        /// <summary>
        /// Get the current assembly Guid.
        /// <remarks>
        /// Note that the Assembly Guid is not necessarily the same as the
        /// Application Guid - if this code is in a DLL, the Assembly Guid
        /// will be the Guid for the DLL, not the active EXE file.
        /// </remarks>
        /// </summary>
        public static Guid AssemblyGuid
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                object[] attr = (asm.GetCustomAttributes(typeof(GuidAttribute), true));
                return new Guid((attr[0] as GuidAttribute).Value);
            }
        }
    }

}