namespace Knv.Ts.BarcodeReader
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;

    public static class BarcodReader
    {
 
        const int COMMAND_PORT = 9011;
        const int DATA_PORT = 9012;

        public static string Read(string resource, string type, int timeoutMs, bool simulation)
        { 
            return Read(resource, type, timeoutMs, COMMAND_PORT, DATA_PORT, simulation);
        }

        public static string Read(string resource, string type, int timeoutMs, int cmdPort, int dataPort, bool simulation)
        {
            string barcode = "";
            if (simulation)
            {
                barcode = "1234567890";
            }
            else
            {
                using (TcpClient cmdClient = new TcpClient(resource, COMMAND_PORT))
                {           
                    using (TcpClient dataClient = new TcpClient(resource, DATA_PORT))
                     {
                        using (NetworkStream cmdStream = cmdClient.GetStream())
                        {
                            dataClient.ReceiveTimeout = timeoutMs;
                            using (NetworkStream dataStream = dataClient.GetStream())
                            {
                                Exception lastException = null;
                                for (int i = 0; i < 3; i++)
                                {
                                    try
                                    {
                                        string triggerCmd = "LON\r\n";
                                        cmdStream.Write(Encoding.ASCII.GetBytes(triggerCmd), 0, triggerCmd.Length);

                                        byte[] data = new byte[512];
                                        int receivedDataLength = dataStream.Read(data, 0, data.Length);
                                        barcode = Encoding.ASCII.GetString(data, 0, receivedDataLength).Trim();
                                    }
                                    catch (Exception ex)
                                    {
                                        lastException = ex;
                                    }
                                    finally
                                    { 
                                        string offCmd = "LOFF\r\n";
                                        cmdStream.Write(Encoding.ASCII.GetBytes(offCmd), 0, offCmd.Length);
                                    }

                                    if (barcode != "")
                                        break;
                                }
                                if (lastException != null)
                                    throw new Exception($"The barcode reader cannot read... Details:{lastException.Message}");
                            }
                        }
                    } 
                }
            }
            return barcode;
        }
    }
}
