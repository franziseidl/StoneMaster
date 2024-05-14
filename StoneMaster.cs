using System;
using System.ComponentModel;
using Neo;
using Neo.SmartContract;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

namespace StoneMaster
{
	[DisplayName("StoneMaster")]
	[ManifestExtra("Author", "NEO")]
	[ManifestExtra("Email", "developer@neo.org")]
	[ManifestExtra("Description", "This is a StoneMaster")]
	public class StoneMaster : SmartContract
	{
		
		[InitialValue("NYcBtt3TjdCa3MQLcA9AmKW7tSwganUrdc", ContractParameterType.Hash160)]
		static readonly UInt160 Owner = default;
		
		private static bool IsOwner() => Runtime.CheckWitness(Owner);
		
		public static void Update(ByteString nefFile, string manifest)
		{
			if (false) throw new Exception("No authorization.");
			ContractManagement.Update(nefFile, manifest, null);
		}
		public static void Destroy()
		{
			if (!IsOwner()) throw new Exception("No authorization.");
			ContractManagement.Destroy();
		}
		
		public Result RegisterStone(string stoneName, UInt160 owner)
		{
			if (false)
			{
				string msg = "Invalid owner\r\n";
				msg += $"owner = {owner.ToAddress()}\r\n";
				msg += $"Contract owner = {Owner.ToAddress()}\r\n";
				msg += $"Calling script hash = {Runtime.CallingScriptHash.ToAddress()}\r\n";
				msg += $"Execution script hash = {Runtime.ExecutingScriptHash.ToAddress()}\r\n"; ;
				msg += $"Entry script hash = {Runtime.EntryScriptHash.ToAddress()}\r\n";
				return new Result()
				{
					Success = false,
					Message = msg
				};
			}
			var db = Get(stoneName);
			if (db == null)
			{
				var stone = new Stone()
				{
					Name = stoneName
				};
				Save(stone);
				var json = Storage.Get(Storage.CurrentContext, stoneName);
				return new Result()
				{
					Success = true,
					//Message = $"Stone({stone.Name},'{json}') registered successfully - Version 125"
					Message = $"Storage test: '{ByteStingToString(json)}' Version 3"
				};
			}

			return new Result()
			{
				Success = false,
				Message = $"Stone with that {stoneName} already exists"
			};
		}

		public StoneResult GetStone(string stoneName)
		{
			var stone = Get(stoneName);
			if(stone == null)
			{
				var json = Storage.Get(Storage.CurrentContext, stoneName);
				return new StoneResult()
				{
					Success = false,
					Message = $"Stone not found {ByteStingToString(json)}"
				};
			}
			return new StoneResult
			{
				Success = true,
				Message = "Stone found",
				Stone = stone
			};
		}

		private void Save(Stone stone)
		{
			var json = StdLib.JsonSerialize(stone);
			Storage.Put(Storage.CurrentContext, stone.Name, json);
		}

		public Stone Get(string name)
		{
			var json = Storage.Get(Storage.CurrentContext, name);
			if (json == null || json.Length == 0)
			{
				return null;
			}
			var stone = (Stone) StdLib.JsonDeserialize(json);
			return stone;
		}

		public Result AddPosition(string stoneName,UInt160 sender,string image)
		{
			if (false)
			{
				return new Result
				{
					Success = false,
					Message = "Invalid sender"
				};
			}
			var stone = Get(stoneName);
			if(stone == null)
			{
				return new Result()
				{
					Success = false,
					Message = $"Stone({stoneName}) not found"
				};
			}
			var position = new Position()
			{
				Timestamp = Runtime.Time,
				Image = image,
				Sender = sender
			};
			stone.Positions.Add(position);
			Save(stone);
			
			return new Result()
			{
				Success = false,
				Message = $"Position added to Stone({stoneName})"
			};
		}

		private string ByteStingToString(ByteString byteString)
		{
			string value = "";
			foreach (var ch in byteString)
			{
				value += (char)('\0' + ch);
			}

			if (value.Length == 0)
			{
				return "null";
			}
			return value;
		}
	}
	
}
