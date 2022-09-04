
using NBitcoin;

public class Builder
{
    public IEnumerable<string> Build()
    {
        var network = Network.TestNet;

        var tx = Transaction.Create(network);

        tx.Inputs.Add(new TxIn
        {
            PrevOut = new OutPoint(uint256.Parse("2d4b1da7d51268ce35a9ad106a6a0b671bbaafc68b673fcaaed0ca804ccbffd0"), 0)
        });
        
        tx.Inputs.Add(new TxIn
        {
            PrevOut = new OutPoint(uint256.Parse("8d6b0b920a80c8583ce52934c03e543e79d331599d6bc1c87c1233c5b2b1d7d0"), 0)
        });


        var outWitAddr = (BitcoinWitPubKeyAddress)BitcoinWitPubKeyAddress.Create("tb1qfg6xrck2rfnpnc29xfxv3nukpxdqrcj22wg6tj", network);
        tx.Outputs.Add(new TxOut
        {
            Value = Money.FromUnit(0.000018M, MoneyUnit.BTC),
            ScriptPubKey = PayToWitPubKeyHashTemplate.Instance.GenerateScriptPubKey(outWitAddr)
        });

        var fromAddr = "tb1qp5l0q2y2xl52wp4a3jq8wp0q93dxr06teh0esx";
        var fromWitAddr = (BitcoinWitPubKeyAddress)BitcoinWitPubKeyAddress.Create(fromAddr, network);
        var inScriptPubKey = PayToWitPubKeyHashTemplate.Instance.GenerateScriptPubKey(fromWitAddr);

        var sigHash1 = tx.GetSignatureHash(inScriptPubKey, 0, SigHash.All, new TxOut(new Money(0.00001M, MoneyUnit.BTC), inScriptPubKey), HashVersion.WitnessV0);
        var sigHash2 = tx.GetSignatureHash(inScriptPubKey, 1, SigHash.All, new TxOut(new Money(0.00001M, MoneyUnit.BTC), inScriptPubKey), HashVersion.WitnessV0);

        Console.WriteLine(inScriptPubKey.ToHex());


        var key = Key.Parse("cVxxBSCxqqj3VebK5WXZhyapG7tfFiz6GmvducnGwXMYePDXqznZ",network);
        TransactionBuilder b = network.CreateTransactionBuilder();
        var tx1 = b
            .AddCoin(new Coin(
                    new OutPoint(uint256.Parse("2d4b1da7d51268ce35a9ad106a6a0b671bbaafc68b673fcaaed0ca804ccbffd0"), 0),
                    new TxOut(Money.FromUnit(0.00001M, MoneyUnit.BTC), inScriptPubKey)))
            .AddKeys(key)
            .AddCoin(new Coin(
                    new OutPoint(uint256.Parse("8d6b0b920a80c8583ce52934c03e543e79d331599d6bc1c87c1233c5b2b1d7d0"), 0),
                    new TxOut(Money.FromUnit(0.00001M, MoneyUnit.BTC), inScriptPubKey)))
            .AddKeys(key)
            .SendFees(Money.FromUnit(0.000002M, MoneyUnit.BTC))
            .Send(outWitAddr, Money.FromUnit(0.000018M, MoneyUnit.BTC))
            .BuildTransaction(true);


        Console.WriteLine(tx1.Inputs[0].WitScript.ToScript());

        Console.WriteLine(b.Verify(tx1, out var transactionPolicyErrors));
        foreach(var err in transactionPolicyErrors)
        {
            Console.WriteLine(err.ToString());
        }
   

        Console.WriteLine(tx1.ToHex());

        return new[] { sigHash1.ToString(), sigHash2.ToString() };
    }
}