using System.Collections;
using System.Management;

namespace IPSwitch
{
    class NetAdapt
    {
        public static ArrayList GetNICNames()
        {
            ArrayList nicNames = new ArrayList();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject m in moc)
            {
                if ((bool)m["ipEnabled"])
                {
                    nicNames.Add(m["caption"]);
                }
            }
            return nicNames;
        }

        public static string GetIp(string nicName)
        {
            string[] ipaddresses = null;
            string ipAddress = "";
            //subnets = null;

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach(ManagementObject m in moc)
            {
                if((bool)m["ipEnabled"])
                {
                    if(m["caption"].Equals(nicName))
                    {
                        ipaddresses = (string[]) m["IPAddress"];
                        //subnets = (string[])m["IPSubnet"];
                        break;
                    }
                }
                
            }
            ipAddress = ipaddresses[0];
            return ipAddress;
        }

        public static string GetMask(string nicName)
        {
            string[] subnets = null;
            string subnet = "";
            //subnets = null;

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject m in moc)
            {
                if ((bool)m["ipEnabled"])
                {
                    if (m["caption"].Equals(nicName))
                    {
                        subnets = (string[])m["IPSubnet"];
                        break;
                    }
                }

            }
            subnet = subnets[0];
            return subnet;
        }

        public static bool CheckDHCP(string nicName)
        {
            bool checkDH = false;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach(ManagementObject m in moc)
            {
               
                    if(m["caption"].Equals(nicName))
                    {
                        if ((bool)m["DHCPEnabled"])
                        {
                            checkDH = true;
                            break;
                        }
                    }
                
            }
            return checkDH;
        }
        public static void SetDHCP(string nicName)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach(ManagementObject m in moc)
            {
                if(m["caption"].Equals(nicName))
                {
                    ManagementBaseObject newDns = m.GetMethodParameters("SetDNSServerSearchOrder");
                    newDns["DNSServerSearchOrder"] = null;
                    ManagementBaseObject enableDHCP = m.InvokeMethod("EnableDHCP", null, null);
                    ManagementBaseObject setDNS = m.InvokeMethod("SetDNSServerSearchOrder", newDns, null);
                    break;
                }
            }
        }
        public static void SetIP(string ipAddress, string subNet, string nicName)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject m in moc)
            {
                if(m["caption"].Equals(nicName))
                {
                    ManagementBaseObject newIP = m.GetMethodParameters("EnableStatic");
                    ManagementBaseObject newGate = m.GetMethodParameters("SetGateways");
                    ManagementBaseObject newDNS = m.GetMethodParameters("SetDNSServerSearchOrder");

                    newIP["IPAddress"] = ipAddress.Split(',');
                    newIP["SubnetMask"] = new string[] { subNet };

                    ManagementBaseObject setIP = m.InvokeMethod("EnableStatic", newIP, null);
                    ManagementBaseObject setGateways = m.InvokeMethod("SetGateways", newGate, null);
                    ManagementBaseObject setDNS = m.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                    break;
                }
            }
        }
    }
}