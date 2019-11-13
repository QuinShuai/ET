using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace ETHotfix {
    public class ByteBuffer {

        private byte[] _shortBytes = new byte[2];
        private byte[] _shortReverseBytes = new byte[2];
        private byte[] _intBytes = new byte[4];
        private byte[] _intReverseBytes = new byte[4];
        private byte[] _longBytes = new byte[8];
        private byte[] _longReverseBytes = new byte[8];
        private byte[] _stringBytes = new byte[65535];
        private IntPtr _shortPtr;
        private IntPtr _intPtr;
        private IntPtr _longPtr;

        public ByteBuffer() {
            _shortPtr = Marshal.UnsafeAddrOfPinnedArrayElement(_shortBytes, 0);
            _intPtr = Marshal.UnsafeAddrOfPinnedArrayElement(_intBytes, 0);
            _longPtr = Marshal.UnsafeAddrOfPinnedArrayElement(_longBytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteShort(MemoryStream ms, short value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(Short2Bytes(value), 0, 2);
            }
            else {
                Short2Bytes(value);
                for (int i = 0; i < 2; i++) {
                    _shortReverseBytes[1 - i] = _shortBytes[i];
                }
                ms.Write(_shortReverseBytes, 0, 2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteUShort(MemoryStream ms, ushort value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(UShort2Bytes(value), 0, 2);
            }
            else {
                UShort2Bytes(value);
                for (int i = 0; i < 2; i++) {
                    _shortReverseBytes[1 - i] = _shortBytes[i];
                }
                ms.Write(_shortReverseBytes, 0, 2);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteInt(MemoryStream ms, int value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(Int2Bytes(value), 0, 4);
            }
            else {
                Int2Bytes(value);
                for (int i = 0; i < 4; i++) {
                    _intReverseBytes[3 - i] = _intBytes[i];
                }
                ms.Write(_intReverseBytes, 0, 4);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteUInt(MemoryStream ms, uint value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(UInt2Bytes(value), 0, 4);
            }
            else {
                UInt2Bytes(value);
                for (int i = 0; i < 4; i++) {
                    _intReverseBytes[3 - i] = _intBytes[i];
                }
                ms.Write(_intReverseBytes, 0, 4);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteLong(MemoryStream ms, long value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(Long2Bytes(value), 0, 8);
            }
            else {
                Long2Bytes(value);
                for (int i = 0; i < 8; i++) {
                    _longReverseBytes[7 - i] = _longBytes[i];
                }
                ms.Write(_longReverseBytes, 0, 8);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteULong(MemoryStream ms, ulong value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(ULong2Bytes(value), 0, 8);
            }
            else {
                ULong2Bytes(value);
                for (int i = 0; i < 8; i++) {
                    _longReverseBytes[7 - i] = _longBytes[i];
                }
                ms.Write(_longReverseBytes, 0, 8);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteFloat(MemoryStream ms, float value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(Float2Bytes(value), 0, 4);
            }
            else {
                Float2Bytes(value);
                for (int i = 0; i < 4; i++) {
                    _intReverseBytes[3 - i] = _intBytes[i];
                }
                ms.Write(_intReverseBytes, 0, 4);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteDouble(MemoryStream ms, double value, bool isLittleEndian = false) {
            if (BitConverter.IsLittleEndian == isLittleEndian) {
                ms.Write(Double2Bytes(value), 0, 8);
            }
            else {
                Double2Bytes(value);
                for (int i = 0; i < 8; i++) {
                    _longReverseBytes[7 - i] = _longBytes[i];
                }
                ms.Write(_longReverseBytes, 0, 8);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="value"></param>
        /// <param name="encodingName"></param>
        /// <param name="isLittleEndian"></param>
        public void WriteString(MemoryStream ms, string value, string encodingName = "utf-8", bool isLittleEndian = false) {
            Encoding encoding = Encoding.GetEncoding(encodingName);
            ushort charLength = (ushort)encoding.GetByteCount(value);
            encoding.GetBytes(value, 0, charLength, _stringBytes, 0);
            WriteUShort(ms, (ushort)charLength, isLittleEndian);
            ms.Write(_stringBytes, 0, charLength);
        }

        //----------------------READ------------------------//
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public short ReadShort(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_shortBytes, 0, 2);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    _shortReverseBytes[1 - i] = _shortBytes[i];
                }
                return BitConverter.ToInt16(_shortReverseBytes, 0);
            }
            else {
                return BitConverter.ToInt16(_shortBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public ushort ReadUShort(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_shortBytes, 0, 2);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    _shortReverseBytes[1 - i] = _shortBytes[i];
                }
                return BitConverter.ToUInt16(_shortReverseBytes, 0);
            }
            else {
                return BitConverter.ToUInt16(_shortBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public int ReadInt(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_intBytes, 0, 4);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    _intReverseBytes[3 - i] = _intBytes[i];
                }
                Debug.Log($"{_intReverseBytes[0]} {_intReverseBytes[1]} {_intReverseBytes[2]} {_intReverseBytes[3]}");
                return BitConverter.ToInt32(_intReverseBytes, 0);
            }
            else {
                Debug.Log($"{_intBytes[0]} {_intBytes[1]} {_intBytes[2]} {_intBytes[3]}");
                return BitConverter.ToInt32(_intBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public uint ReadUInt(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_intBytes, 0, 4);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    _intReverseBytes[3 - i] = _intBytes[i];
                }
                return BitConverter.ToUInt32(_intReverseBytes, 0);
            }
            else {
                return BitConverter.ToUInt32(_intBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public long ReadLong(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_longBytes, 0, 8);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 8; i++) {
                    _longReverseBytes[7 - i] = _longBytes[i];
                }
                return BitConverter.ToInt64(_longReverseBytes, 0);
            }
            else {
                return BitConverter.ToInt64(_longBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public ulong ReadULong(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_longBytes, 0, 8);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 8; i++) {
                    _longReverseBytes[7 - i] = _longBytes[i];
                }
                return BitConverter.ToUInt64(_longReverseBytes, 0);
            }
            else {
                return BitConverter.ToUInt64(_longBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public float ReadFloat(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_intBytes, 0, 4);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    _intReverseBytes[3 - i] = _intBytes[i];
                }
                return BitConverter.ToSingle(_intReverseBytes, 0);
            }
            else {
                return BitConverter.ToSingle(_intBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public double ReadDouble(MemoryStream ms, bool isLittleEndian = false) {
            ms.Read(_longBytes, 0, 8);
            if (BitConverter.IsLittleEndian != isLittleEndian) {
                for (int i = 0; i < 8; i++) {
                    _longReverseBytes[7 - i] = _longBytes[i];
                }
                return BitConverter.ToDouble(_longReverseBytes, 0);
            }
            else {
                return BitConverter.ToDouble(_longBytes, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="encodingName"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public string ReadString(MemoryStream ms, string encodingName = "utf-8", bool isLittleEndian = false) {
            ushort length = ReadUShort(ms, isLittleEndian);
            return ReadString(ms, length, encodingName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="length"></param>
        /// <param name="encodingName"></param>
        /// <returns></returns>
        public string ReadString(MemoryStream ms, int length, string encodingName = "utf-8") {
            Encoding encoding = Encoding.GetEncoding(encodingName);
            ms.Read(_stringBytes, 0, length);
            string str = encoding.GetString(_stringBytes, 0, length);
            return str;
        }


        //----------------------ToBytes------------------------//


        private byte[] Float2Bytes(float val) {
            Marshal.StructureToPtr((object)val, _intPtr, true);
            return _intBytes;
        }

        private byte[] Double2Bytes(double val) {
            Marshal.StructureToPtr((object)val, _longPtr, true);
            return _longBytes;
        }

        private byte[] Short2Bytes(short val) {
            Marshal.WriteInt16(_shortPtr, val);
            return _shortBytes;
        }
        private byte[] UShort2Bytes(ushort val) {
            return Short2Bytes((short)val);
        }
        private byte[] Int2Bytes(int val) {
            Marshal.WriteInt32(_intPtr, val);
            return _intBytes;
        }

        private byte[] UInt2Bytes(uint val) {
            return Int2Bytes((int)val);
        }
        private byte[] Long2Bytes(long val) {
            Marshal.WriteInt64(_longPtr, val);
            return _longBytes;
        }
        private byte[] ULong2Bytes(ulong val) {
            return Long2Bytes((long)val);
        }
    }
}
