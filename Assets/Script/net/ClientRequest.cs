using System;
using System.IO;
using UnityEngine;
using System.Text;

namespace AssemblyCSharp
{
	public class ClientRequest
	{
		private byte Flag = 1;
		protected int Len = 0;
		public int headCode;
		private short msgLen;
		public string msg="";
		public int totelLenght;
		public ClientRequest ()
		{
		}
		public ClientRequest (int cmd,string msg)
		{
			this.headCode = cmd;
			this.msg = msg;
		}
		public void setData(){

		}



		/// <summary>
		/// 写入大端序的int
		/// </summary>
		/// <param name="value"></param>
		public byte[] WriterInt(int value)
		{
			byte[] bs = BitConverter.GetBytes(value);
			Array.Reverse(bs);
			return bs;
		}
		/// <summary>
		/// Writes the short.
		/// </summary>
		/// <returns>The short.</returns>
		/// <param name="value">Value.</param>
		public byte[] WriteShort(short value){
			byte[] bs = BitConverter.GetBytes(value);
			Array.Reverse(bs);
			return bs;
		}

		public byte[] WriterString(string value){

			byte[] result = Encoding.UTF8.GetBytes (value);
			return result;
		}
	
		/// <summary>
		///  转换为 byte[]
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			byte[] _bytes; //自定义字节数组，用以装载消息协议
			using (MemoryStream memoryStream = new MemoryStream()) //创建内存流
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream,UTF8Encoding.Default); //以二进制写入器往这个流里写内容
				msgLen = (short)Encoding.UTF8.GetBytes(msg).Length;
				if (msgLen > 0) {
					Len =( 6 + msgLen);
				} else {
					Len =( 4 + msgLen);
				}
				binaryWriter.Write(Flag); //写入协议一级标志，占1个字节
				binaryWriter.Write(WriterInt(Len));//占4个字节
				binaryWriter.Write(WriterInt(headCode)); //写入实际消息长度，占4个字节
				if (msgLen > 0)
				{
					binaryWriter.Write (WriteShort(msgLen));
					binaryWriter.Write(WriterString( msg)); //写入实际消息内容
				}
				_bytes = memoryStream.ToArray(); //将流内容写入自定义字节数组
				binaryWriter.Close(); //关闭写入器释放资源
			}
			totelLenght = _bytes.Length;
			return _bytes; //返回填充好消息协议对象的自定义字节数组
		}
	}
}

