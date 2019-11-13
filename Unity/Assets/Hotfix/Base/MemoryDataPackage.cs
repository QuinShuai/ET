using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ETHotfix {
    /// <summary>
    /// 
    /// </summary>
    public class MemoryDataPackage : IDisposable {

        private static Stack<MemoryStream> _memoryStreamStack = new Stack<MemoryStream>();
        private static Stack<ByteBuffer> _byteBufferStack = new Stack<ByteBuffer>();

        private static int _maxPoolLength = 60;
        /// <summary>
        /// 
        /// </summary>
        public static int MaxPoolLength {
            get {
                return _maxPoolLength;
            }
            set {
                _maxPoolLength = value;

                lock (_memoryStreamStack) {
                    if (_maxPoolLength <= 0) {
                        _maxPoolLength = 0;
                        _memoryStreamStack.Clear();
                        _byteBufferStack.Clear();
                    }
                    while (_memoryStreamStack.Count > _maxPoolLength) {
                        _memoryStreamStack.Pop();
                        _byteBufferStack.Pop();
                    }
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void ClearPool() {
            lock (_memoryStreamStack) {
                _memoryStreamStack.Clear();
                _byteBufferStack.Clear();
            }
        }
        private ByteBuffer _byteBuffer;

        private MemoryStream _memoryStream;
        /// <summary>
        /// 
        /// </summary>
        public MemoryStream memoryStream {
            get {
                return _memoryStream;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Position {
            get {
                return _memoryStream.Position;
            }
            set {
                _memoryStream.Position = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Length {
            get {
                return _memoryStream.Length;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetLength(long value) {
            _memoryStream.SetLength(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool isLittleEndian = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isLittleEndian"></param>
        public MemoryDataPackage(bool isLittleEndian = false) {
            this.isLittleEndian = isLittleEndian;
            lock (_memoryStreamStack) {
                if (_memoryStreamStack.Count > 0) {
                    _memoryStream = _memoryStreamStack.Pop();
                    _byteBuffer = _byteBufferStack.Pop();
                }
                else {
                    _memoryStream = new MemoryStream(4096);
                    _byteBuffer = new ByteBuffer();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            lock (_memoryStreamStack) {
                _memoryStream.Position = 0;
                _memoryStream.SetLength(0);
                _memoryStreamStack.Push(_memoryStream);
                _byteBufferStack.Push(_byteBuffer);
                _memoryStream = null;
                _byteBuffer = null;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public T ProtoBufDeserialize<T>() {
        //    return Serializer.Deserialize<T>(_memoryStream);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="instance"></param>
        //public void ProtoBufSerialize(object instance) {
        //    Serializer.Serialize(_memoryStream, instance);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteShort(short value) {
            _byteBuffer.WriteShort(_memoryStream, value, isLittleEndian);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteUShort(ushort value) {
            _byteBuffer.WriteUShort(_memoryStream, value, isLittleEndian);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt(int value) {
            _byteBuffer.WriteInt(_memoryStream, value, isLittleEndian);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt(uint value) {
            _byteBuffer.WriteUInt(_memoryStream, value, isLittleEndian);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(long value) {
            _byteBuffer.WriteLong(_memoryStream, value, isLittleEndian);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteULong(ulong value) {
            _byteBuffer.WriteULong(_memoryStream, value, isLittleEndian);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteFloat(float value) {
            _byteBuffer.WriteFloat(_memoryStream, value, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteDouble(double value) {
            _byteBuffer.WriteDouble(_memoryStream, value, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encodingName"></param>
        public void WriteString(string value, string encodingName = "utf-8") {
            _byteBuffer.WriteString(_memoryStream, value, encodingName, isLittleEndian);
        }

        //----------------------READ------------------------//
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public short ReadShort() {
            return _byteBuffer.ReadShort(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort() {
            return _byteBuffer.ReadUShort(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadInt() {
            return _byteBuffer.ReadInt(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt() {
            return _byteBuffer.ReadUInt(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long ReadLong() {
            return _byteBuffer.ReadLong(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ulong ReadULong() {
            return _byteBuffer.ReadULong(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float ReadFloat() {
            return _byteBuffer.ReadFloat(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double ReadDouble() {
            return _byteBuffer.ReadDouble(_memoryStream, isLittleEndian);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodingName"></param>
        /// <returns></returns>
        public string ReadString(string encodingName = "utf-8") {
            var length = _byteBuffer.ReadInt(_memoryStream, isLittleEndian);
           //Debug.Log(length);
            return _byteBuffer.ReadString(_memoryStream, length, encodingName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="encodingName"></param>
        /// <returns></returns>
        public string ReadString(ushort length, string encodingName = "utf-8") {
            return _byteBuffer.ReadString(_memoryStream, length, encodingName);
        }

    }
}