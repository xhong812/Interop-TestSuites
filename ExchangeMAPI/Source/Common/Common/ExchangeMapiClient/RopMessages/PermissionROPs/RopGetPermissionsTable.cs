namespace Microsoft.Protocols.TestSuites.Common
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// RopGetPermissionsTable request buffer structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]

    public struct RopGetPermissionsTableRequest : ISerializable
    {
        /// <summary>
        /// This value specifies the type of remote operation. For this operation, this field is set to 0x3E.
        /// </summary>
        public byte RopId;

        /// <summary>
        /// This value specifies the logon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// This index specifies the location in the Server Object Handle Table 
        /// where the handle for the input Server Object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// This index specifies the location in the Server Object Handle Table 
        /// where the handle for the output Server Object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// The possible values are specified in [MS-OXCPERM]. These flags control the type of table.
        /// </summary>
        public byte TableFlags;
        
        /// <summary>
        /// Serialize the ROP request buffer.
        /// </summary>
        /// <returns>The ROP request buffer serialized.</returns>
        public byte[] Serialize()
        {
            byte[] serializeBuffer = new byte[Marshal.SizeOf(this)];
            IntPtr requestBuffer = new IntPtr();
            requestBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(this));
            try
            {
                Marshal.StructureToPtr(this, requestBuffer, true);
                Marshal.Copy(requestBuffer, serializeBuffer, 0, Marshal.SizeOf(this));
                return serializeBuffer;
            }
            finally
            {
                Marshal.FreeHGlobal(requestBuffer);
            }
        }

        /// <summary>
        /// Return the size of this structure.
        /// </summary>
        /// <returns>The size of this structure.</returns>
        public int Size()
        {
            return Marshal.SizeOf(this);
        }
    }

    /// <summary>
    /// RopGetPermissionTable response buffer structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RopGetPermissionsTableResponse : IDeserializable
    {
        /// <summary>
        /// This value specifies the type of remote operation. For this operation, this field is set to 0x3E.
        /// </summary>
        public byte RopId;

        /// <summary>
        /// This index MUST be set to the OutputHandleIndex specified in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// This value specifies the status of the remote operation.
        /// </summary>
        public uint ReturnValue;

        /// <summary>
        /// Deserialize the ROP response buffer.
        /// </summary>
        /// <param name="ropBytes">ROPs bytes in response.</param>
        /// <param name="startIndex">The start index of this ROP.</param>
        /// <returns>The size of response buffer structure.</returns>
        public int Deserialize(byte[] ropBytes, int startIndex)
        {
            IntPtr responseBuffer = new IntPtr();
            responseBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(this));
            try
            {
                Marshal.Copy(ropBytes, startIndex, responseBuffer, Marshal.SizeOf(this));
                this = (RopGetPermissionsTableResponse)Marshal.PtrToStructure(responseBuffer, typeof(RopGetPermissionsTableResponse));
                return Marshal.SizeOf(this);
            }
            finally
            {
                Marshal.FreeHGlobal(responseBuffer);
            }
        }
    }
}