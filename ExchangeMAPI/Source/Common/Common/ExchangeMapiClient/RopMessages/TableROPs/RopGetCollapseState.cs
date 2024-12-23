namespace Microsoft.Protocols.TestSuites.Common
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// RopGetCollapseState request buffer structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RopGetCollapseStateRequest : ISerializable
    {
        /// <summary>
        /// Unsigned 8-bit integer. This value specifies the type of remote operation. 
        /// For this operation, this field is set to 0x6B.
        /// </summary>
        public byte RopId;

        /// <summary>
        /// Unsigned 8-bit integer. This value specifies the logon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// Unsigned 8-bit integer. This index specifies the location in the Server Object Handle Table 
        /// where the handle for the input Server Object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// 64-bit identifier. This identifier specifies the row to be preserved as the cursor. 
        /// The cursor is returned as part of the collapse state in the CollapseState field of the response.
        /// </summary>
        public ulong RowId;

        /// <summary>
        /// Unsigned 32-bit integer. This value specifies the instance number of the row that is to be preserved as the cursor.
        /// </summary>
        public uint RowInstanceNumber;

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
    /// RopGetCollapseState response buffer structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RopGetCollapseStateResponse : IDeserializable
    {
        /// <summary>
        /// Unsigned 8-bit integer. This value specifies the type of remote operation. 
        /// For this operation, this field is set to 0x6B.
        /// </summary>
        public byte RopId;

        /// <summary>
        /// Unsigned 8-bit integer. This index MUST be set to the InputHandleIndex specified in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Unsigned 32-bit integer. This value specifies the status of the remote operation.
        /// For success response, this field is set to 0x00000000.
        /// For failure response, this field is set to a value other than 0x00000000.
        /// </summary>
        public uint ReturnValue;

        /// <summary>
        /// Unsigned 16-bit integer. This value specifies the size of the CollapseState field.
        /// </summary>
        public ushort CollapseStateSize;

        /// <summary>
        /// Array of bytes. The size of this field, in bytes, is specified by the CollapseStateSize field. 
        /// This array specifies a collapse state for a categorized table.
        /// </summary>
        public byte[] CollapseState;

        /// <summary>
        /// Deserialize the ROP response buffer.
        /// </summary>
        /// <param name="ropBytes">ROPs bytes in response.</param>
        /// <param name="startIndex">The start index of this ROP.</param>
        /// <returns>The size of response buffer structure.</returns>
        public int Deserialize(byte[] ropBytes, int startIndex)
        {
            int index = startIndex;
            this.RopId = ropBytes[index++];
            this.InputHandleIndex = ropBytes[index++];
            this.ReturnValue = (uint)BitConverter.ToInt32(ropBytes, index);
            index += sizeof(uint);

            // Only success response has below fields
            if (this.ReturnValue == 0)
            {
                this.CollapseStateSize = (ushort)BitConverter.ToInt16(ropBytes, index);
                index += sizeof(ushort);
                this.CollapseState = new byte[this.CollapseStateSize];
                Array.Copy(ropBytes, index, this.CollapseState, 0, this.CollapseStateSize);
                index += this.CollapseStateSize;
            }

            return index - startIndex;
        }
    }
}