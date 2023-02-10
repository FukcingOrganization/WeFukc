using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Contracts.Contracts.Lord.ContractDefinition
{


    public partial class LordDeployment : LordDeploymentBase
    {
        public LordDeployment() : base(BYTECODE) { }
        public LordDeployment(string byteCode) : base(byteCode) { }
    }

    public class LordDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x";
        public LordDeploymentBase() : base(BYTECODE) { }
        public LordDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address[13]", "_contracts", 1)]
        public virtual List<string> Contracts { get; set; }
    }

    public partial class DAOvoteFunction : DAOvoteFunctionBase { }

    [Function("DAOvote")]
    public class DAOvoteFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
        [Parameter("bool", "_isApproving", 2)]
        public virtual bool IsApproving { get; set; }
        [Parameter("uint256", "_lordID", 3)]
        public virtual BigInteger LordID { get; set; }
    }

    public partial class DEBUG_setContractFunction : DEBUG_setContractFunctionBase { }

    [Function("DEBUG_setContract")]
    public class DEBUG_setContractFunctionBase : FunctionMessage
    {
        [Parameter("address", "_contractAddress", 1)]
        public virtual string ContractAddress { get; set; }
        [Parameter("uint256", "_index", 2)]
        public virtual BigInteger Index { get; set; }
    }

    public partial class DEBUG_setContractsFunction : DEBUG_setContractsFunctionBase { }

    [Function("DEBUG_setContracts")]
    public class DEBUG_setContractsFunctionBase : FunctionMessage
    {
        [Parameter("address[13]", "_contracts", 1)]
        public virtual List<string> Contracts { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class BaseMintCostFunction : BaseMintCostFunctionBase { }

    [Function("baseMintCost", "uint256")]
    public class BaseMintCostFunctionBase : FunctionMessage
    {

    }

    public partial class BaseTaxRateFunction : BaseTaxRateFunctionBase { }

    [Function("baseTaxRate", "uint256")]
    public class BaseTaxRateFunctionBase : FunctionMessage
    {

    }

    public partial class BurnFunction : BurnFunctionBase { }

    [Function("burn")]
    public class BurnFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ClaimRebellionRewardsFunction : ClaimRebellionRewardsFunctionBase { }

    [Function("claimRebellionRewards")]
    public class ClaimRebellionRewardsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_rebellionID", 1)]
        public virtual BigInteger RebellionID { get; set; }
        [Parameter("uint256", "_lordID", 2)]
        public virtual BigInteger LordID { get; set; }
    }

    public partial class ClanRegistrationFunction : ClanRegistrationFunctionBase { }

    [Function("clanRegistration")]
    public class ClanRegistrationFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
        [Parameter("uint256", "_clanID", 2)]
        public virtual BigInteger ClanID { get; set; }
    }

    public partial class ClansOfFunction : ClansOfFunctionBase { }

    [Function("clansOf", "uint256")]
    public class ClansOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class ContractsFunction : ContractsFunctionBase { }

    [Function("contracts", "address")]
    public class ContractsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ExecuteBaseTaxRateProposalFunction : ExecuteBaseTaxRateProposalFunctionBase { }

    [Function("executeBaseTaxRateProposal")]
    public class ExecuteBaseTaxRateProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteContractAddressUpdateProposalFunction : ExecuteContractAddressUpdateProposalFunctionBase { }

    [Function("executeContractAddressUpdateProposal")]
    public class ExecuteContractAddressUpdateProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteFunctionsProposalTypesUpdateProposalFunction : ExecuteFunctionsProposalTypesUpdateProposalFunctionBase { }

    [Function("executeFunctionsProposalTypesUpdateProposal")]
    public class ExecuteFunctionsProposalTypesUpdateProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteRebellionLengthProposalFunction : ExecuteRebellionLengthProposalFunctionBase { }

    [Function("executeRebellionLengthProposal")]
    public class ExecuteRebellionLengthProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteSignalLengthProposalFunction : ExecuteSignalLengthProposalFunctionBase { }

    [Function("executeSignalLengthProposal")]
    public class ExecuteSignalLengthProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteTaxRateChangeProposalFunction : ExecuteTaxRateChangeProposalFunctionBase { }

    [Function("executeTaxRateChangeProposal")]
    public class ExecuteTaxRateChangeProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteVictoryRateProposalFunction : ExecuteVictoryRateProposalFunctionBase { }

    [Function("executeVictoryRateProposal")]
    public class ExecuteVictoryRateProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteWarCasualtyRateProposalFunction : ExecuteWarCasualtyRateProposalFunctionBase { }

    [Function("executeWarCasualtyRateProposal")]
    public class ExecuteWarCasualtyRateProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class FunctionsProposalTypesFunction : FunctionsProposalTypesFunctionBase { }

    [Function("functionsProposalTypes", "uint256")]
    public class FunctionsProposalTypesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class FundLordFunction : FundLordFunctionBase { }

    [Function("fundLord")]
    public class FundLordFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
        [Parameter("uint256", "_amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class FundRebelsFunction : FundRebelsFunctionBase { }

    [Function("fundRebels")]
    public class FundRebelsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
        [Parameter("uint256", "_amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class GetApprovedFunction : GetApprovedFunctionBase { }

    [Function("getApproved", "address")]
    public class GetApprovedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class IsApprovedForAllFunction : IsApprovedForAllFunctionBase { }

    [Function("isApprovedForAll", "bool")]
    public class IsApprovedForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "operator", 2)]
        public virtual string Operator { get; set; }
    }

    public partial class IsClanSignalledFunction : IsClanSignalledFunctionBase { }

    [Function("isClanSignalled", "bool")]
    public class IsClanSignalledFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_rebellionNumber", 1)]
        public virtual BigInteger RebellionNumber { get; set; }
        [Parameter("uint256", "_clanID", 2)]
        public virtual BigInteger ClanID { get; set; }
    }

    public partial class IsRentedFunction : IsRentedFunctionBase { }

    [Function("isRented", "bool")]
    public class IsRentedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
    }

    public partial class LordMintFunction : LordMintFunctionBase { }

    [Function("lordMint")]
    public class LordMintFunctionBase : FunctionMessage
    {

    }

    public partial class LordTaxInfoFunction : LordTaxInfoFunctionBase { }

    [Function("lordTaxInfo", typeof(LordTaxInfoOutputDTO))]
    public class LordTaxInfoFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
    }

    public partial class MaxSupplyFunction : MaxSupplyFunctionBase { }

    [Function("maxSupply", "uint256")]
    public class MaxSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class MintClanLicenseFunction : MintClanLicenseFunctionBase { }

    [Function("mintClanLicense")]
    public class MintClanLicenseFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
        [Parameter("uint256", "_amount", 2)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bytes", "_data", 3)]
        public virtual byte[] Data { get; set; }
    }

    public partial class MintCostIncrementFunction : MintCostIncrementFunctionBase { }

    [Function("mintCostIncrement", "uint256")]
    public class MintCostIncrementFunctionBase : FunctionMessage
    {

    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class NumberOfClansFunction : NumberOfClansFunctionBase { }

    [Function("numberOfClans", "uint256")]
    public class NumberOfClansFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class NumberOfGloriesFunction : NumberOfGloriesFunctionBase { }

    [Function("numberOfGlories", "uint256")]
    public class NumberOfGloriesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class OwnerOfFunction : OwnerOfFunctionBase { }

    [Function("ownerOf", "address")]
    public class OwnerOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ProposalsFunction : ProposalsFunctionBase { }

    [Function("proposals", typeof(ProposalsOutputDTO))]
    public class ProposalsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ProposeBaseTaxRateUpdateFunction : ProposeBaseTaxRateUpdateFunctionBase { }

    [Function("proposeBaseTaxRateUpdate")]
    public class ProposeBaseTaxRateUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_newBaseTaxRate", 1)]
        public virtual BigInteger NewBaseTaxRate { get; set; }
    }

    public partial class ProposeContractAddressUpdateFunction : ProposeContractAddressUpdateFunctionBase { }

    [Function("proposeContractAddressUpdate")]
    public class ProposeContractAddressUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_contractIndex", 1)]
        public virtual BigInteger ContractIndex { get; set; }
        [Parameter("address", "_newAddress", 2)]
        public virtual string NewAddress { get; set; }
    }

    public partial class ProposeFunctionsProposalTypesUpdateFunction : ProposeFunctionsProposalTypesUpdateFunctionBase { }

    [Function("proposeFunctionsProposalTypesUpdate")]
    public class ProposeFunctionsProposalTypesUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_functionIndex", 1)]
        public virtual BigInteger FunctionIndex { get; set; }
        [Parameter("uint256", "_newIndex", 2)]
        public virtual BigInteger NewIndex { get; set; }
    }

    public partial class ProposeRebellionLengthUpdateFunction : ProposeRebellionLengthUpdateFunctionBase { }

    [Function("proposeRebellionLengthUpdate")]
    public class ProposeRebellionLengthUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_newRebellionLength", 1)]
        public virtual BigInteger NewRebellionLength { get; set; }
    }

    public partial class ProposeSignalLengthUpdateFunction : ProposeSignalLengthUpdateFunctionBase { }

    [Function("proposeSignalLengthUpdate")]
    public class ProposeSignalLengthUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_newSignalLength", 1)]
        public virtual BigInteger NewSignalLength { get; set; }
    }

    public partial class ProposeTaxChangeRateUpdateFunction : ProposeTaxChangeRateUpdateFunctionBase { }

    [Function("proposeTaxChangeRateUpdate")]
    public class ProposeTaxChangeRateUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_newTaxChangeRate", 1)]
        public virtual BigInteger NewTaxChangeRate { get; set; }
    }

    public partial class ProposeVictoryRateUpdateFunction : ProposeVictoryRateUpdateFunctionBase { }

    [Function("proposeVictoryRateUpdate")]
    public class ProposeVictoryRateUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_newVictoryRate", 1)]
        public virtual BigInteger NewVictoryRate { get; set; }
    }

    public partial class ProposeWarCasualtyRateUpdateFunction : ProposeWarCasualtyRateUpdateFunctionBase { }

    [Function("proposeWarCasualtyRateUpdate")]
    public class ProposeWarCasualtyRateUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_newWarCasualtyRate", 1)]
        public virtual BigInteger NewWarCasualtyRate { get; set; }
    }

    public partial class RebellionLengthFunction : RebellionLengthFunctionBase { }

    [Function("rebellionLength", "uint256")]
    public class RebellionLengthFunctionBase : FunctionMessage
    {

    }

    public partial class RebellionOfFunction : RebellionOfFunctionBase { }

    [Function("rebellionOf", "uint256")]
    public class RebellionOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class RebellionsFunction : RebellionsFunctionBase { }

    [Function("rebellions", typeof(RebellionsOutputDTO))]
    public class RebellionsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class SafeTransferFrom1Function : SafeTransferFrom1FunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFrom1FunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SetApprovalForAllFunction : SetApprovalForAllFunctionBase { }

    [Function("setApprovalForAll")]
    public class SetApprovalForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "operator", 1)]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 2)]
        public virtual bool Approved { get; set; }
    }

    public partial class SetBaseURIFunction : SetBaseURIFunctionBase { }

    [Function("setBaseURI")]
    public class SetBaseURIFunctionBase : FunctionMessage
    {
        [Parameter("string", "_newURI", 1)]
        public virtual string NewURI { get; set; }
    }

    public partial class SetCustomLicenseURIFunction : SetCustomLicenseURIFunctionBase { }

    [Function("setCustomLicenseURI")]
    public class SetCustomLicenseURIFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
        [Parameter("string", "_newURI", 2)]
        public virtual string NewURI { get; set; }
    }

    public partial class SetCustomLordURIFunction : SetCustomLordURIFunctionBase { }

    [Function("setCustomLordURI")]
    public class SetCustomLordURIFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordId", 1)]
        public virtual BigInteger LordId { get; set; }
        [Parameter("string", "_customURI", 2)]
        public virtual string CustomURI { get; set; }
    }

    public partial class SetUserFunction : SetUserFunctionBase { }

    [Function("setUser")]
    public class SetUserFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "user", 2)]
        public virtual string User { get; set; }
        [Parameter("uint256", "expires", 3)]
        public virtual BigInteger Expires { get; set; }
    }

    public partial class SignalLengthFunction : SignalLengthFunctionBase { }

    [Function("signalLength", "uint256")]
    public class SignalLengthFunctionBase : FunctionMessage
    {

    }

    public partial class SignalRebellionFunction : SignalRebellionFunctionBase { }

    [Function("signalRebellion")]
    public class SignalRebellionFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordID", 1)]
        public virtual BigInteger LordID { get; set; }
        [Parameter("uint256", "_clanID", 2)]
        public virtual BigInteger ClanID { get; set; }
    }

    public partial class SupportsInterfaceFunction : SupportsInterfaceFunctionBase { }

    [Function("supportsInterface", "bool")]
    public class SupportsInterfaceFunctionBase : FunctionMessage
    {
        [Parameter("bytes4", "interfaceId", 1)]
        public virtual byte[] InterfaceId { get; set; }
    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TaxChangeRateFunction : TaxChangeRateFunctionBase { }

    [Function("taxChangeRate", "uint256")]
    public class TaxChangeRateFunctionBase : FunctionMessage
    {

    }

    public partial class TokenURIFunction : TokenURIFunctionBase { }

    [Function("tokenURI", "string")]
    public class TokenURIFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_lordId", 1)]
        public virtual BigInteger LordId { get; set; }
    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class UserExpiresFunction : UserExpiresFunctionBase { }

    [Function("userExpires", "uint256")]
    public class UserExpiresFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class UserOfFunction : UserOfFunctionBase { }

    [Function("userOf", "address")]
    public class UserOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class VictoryRateFunction : VictoryRateFunctionBase { }

    [Function("victoryRate", "uint256")]
    public class VictoryRateFunctionBase : FunctionMessage
    {

    }

    public partial class ViewLordBackerFundFunction : ViewLordBackerFundFunctionBase { }

    [Function("viewLordBackerFund", "uint256")]
    public class ViewLordBackerFundFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_rebellionNumber", 1)]
        public virtual BigInteger RebellionNumber { get; set; }
        [Parameter("address", "_backerAddress", 2)]
        public virtual string BackerAddress { get; set; }
    }

    public partial class ViewRebelBackerFundFunction : ViewRebelBackerFundFunctionBase { }

    [Function("viewRebelBackerFund", "uint256")]
    public class ViewRebelBackerFundFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_rebellionNumber", 1)]
        public virtual BigInteger RebellionNumber { get; set; }
        [Parameter("address", "_backerAddress", 2)]
        public virtual string BackerAddress { get; set; }
    }

    public partial class ViewRebellionStatusFunction : ViewRebellionStatusFunctionBase { }

    [Function("viewRebellionStatus", "uint256")]
    public class ViewRebellionStatusFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_rebellionNumber", 1)]
        public virtual BigInteger RebellionNumber { get; set; }
    }

    public partial class WarCasualtyRateFunction : WarCasualtyRateFunctionBase { }

    [Function("warCasualtyRate", "uint256")]
    public class WarCasualtyRateFunctionBase : FunctionMessage
    {

    }

    public partial class WithdrawLpFundsFunction : WithdrawLpFundsFunctionBase { }

    [Function("withdrawLpFunds")]
    public class WithdrawLpFundsFunctionBase : FunctionMessage
    {

    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "approved", 2, true )]
        public virtual string Approved { get; set; }
        [Parameter("uint256", "tokenId", 3, true )]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase { }

    [Event("ApprovalForAll")]
    public class ApprovalForAllEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "operator", 2, true )]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 3, false )]
        public virtual bool Approved { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3, true )]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class UpdateUserEventDTO : UpdateUserEventDTOBase { }

    [Event("UpdateUser")]
    public class UpdateUserEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "tokenId", 1, true )]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "user", 2, true )]
        public virtual string User { get; set; }
        [Parameter("uint256", "expires", 3, false )]
        public virtual BigInteger Expires { get; set; }
    }









    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class BaseMintCostOutputDTO : BaseMintCostOutputDTOBase { }

    [FunctionOutput]
    public class BaseMintCostOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class BaseTaxRateOutputDTO : BaseTaxRateOutputDTOBase { }

    [FunctionOutput]
    public class BaseTaxRateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }







    public partial class ClansOfOutputDTO : ClansOfOutputDTOBase { }

    [FunctionOutput]
    public class ClansOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ContractsOutputDTO : ContractsOutputDTOBase { }

    [FunctionOutput]
    public class ContractsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

















    public partial class FunctionsProposalTypesOutputDTO : FunctionsProposalTypesOutputDTOBase { }

    [FunctionOutput]
    public class FunctionsProposalTypesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }





    public partial class GetApprovedOutputDTO : GetApprovedOutputDTOBase { }

    [FunctionOutput]
    public class GetApprovedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class IsApprovedForAllOutputDTO : IsApprovedForAllOutputDTOBase { }

    [FunctionOutput]
    public class IsApprovedForAllOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class IsClanSignalledOutputDTO : IsClanSignalledOutputDTOBase { }

    [FunctionOutput]
    public class IsClanSignalledOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class IsRentedOutputDTO : IsRentedOutputDTOBase { }

    [FunctionOutput]
    public class IsRentedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }



    public partial class LordTaxInfoOutputDTO : LordTaxInfoOutputDTOBase { }

    [FunctionOutput]
    public class LordTaxInfoOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class MaxSupplyOutputDTO : MaxSupplyOutputDTOBase { }

    [FunctionOutput]
    public class MaxSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class MintCostIncrementOutputDTO : MintCostIncrementOutputDTOBase { }

    [FunctionOutput]
    public class MintCostIncrementOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class NumberOfClansOutputDTO : NumberOfClansOutputDTOBase { }

    [FunctionOutput]
    public class NumberOfClansOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class NumberOfGloriesOutputDTO : NumberOfGloriesOutputDTOBase { }

    [FunctionOutput]
    public class NumberOfGloriesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class OwnerOfOutputDTO : OwnerOfOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ProposalsOutputDTO : ProposalsOutputDTOBase { }

    [FunctionOutput]
    public class ProposalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "status", 1)]
        public virtual byte Status { get; set; }
        [Parameter("uint256", "updateCode", 2)]
        public virtual BigInteger UpdateCode { get; set; }
        [Parameter("bool", "isExecuted", 3)]
        public virtual bool IsExecuted { get; set; }
        [Parameter("uint256", "index", 4)]
        public virtual BigInteger Index { get; set; }
        [Parameter("uint256", "newUint", 5)]
        public virtual BigInteger NewUint { get; set; }
        [Parameter("address", "newAddress", 6)]
        public virtual string NewAddress { get; set; }
        [Parameter("bytes32", "newBytes32", 7)]
        public virtual byte[] NewBytes32 { get; set; }
        [Parameter("bool", "newBool", 8)]
        public virtual bool NewBool { get; set; }
    }

















    public partial class RebellionLengthOutputDTO : RebellionLengthOutputDTOBase { }

    [FunctionOutput]
    public class RebellionLengthOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class RebellionOfOutputDTO : RebellionOfOutputDTOBase { }

    [FunctionOutput]
    public class RebellionOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class RebellionsOutputDTO : RebellionsOutputDTOBase { }

    [FunctionOutput]
    public class RebellionsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "status", 1)]
        public virtual byte Status { get; set; }
        [Parameter("uint256", "startDate", 2)]
        public virtual BigInteger StartDate { get; set; }
        [Parameter("uint256", "lordFunds", 3)]
        public virtual BigInteger LordFunds { get; set; }
        [Parameter("uint256", "rebelFunds", 4)]
        public virtual BigInteger RebelFunds { get; set; }
        [Parameter("uint256", "totalFunds", 5)]
        public virtual BigInteger TotalFunds { get; set; }
        [Parameter("uint256", "numberOfSignaledClans", 6)]
        public virtual BigInteger NumberOfSignaledClans { get; set; }
    }















    public partial class SignalLengthOutputDTO : SignalLengthOutputDTOBase { }

    [FunctionOutput]
    public class SignalLengthOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class SupportsInterfaceOutputDTO : SupportsInterfaceOutputDTOBase { }

    [FunctionOutput]
    public class SupportsInterfaceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TaxChangeRateOutputDTO : TaxChangeRateOutputDTOBase { }

    [FunctionOutput]
    public class TaxChangeRateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TokenURIOutputDTO : TokenURIOutputDTOBase { }

    [FunctionOutput]
    public class TokenURIOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class UserExpiresOutputDTO : UserExpiresOutputDTOBase { }

    [FunctionOutput]
    public class UserExpiresOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class UserOfOutputDTO : UserOfOutputDTOBase { }

    [FunctionOutput]
    public class UserOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class VictoryRateOutputDTO : VictoryRateOutputDTOBase { }

    [FunctionOutput]
    public class VictoryRateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ViewLordBackerFundOutputDTO : ViewLordBackerFundOutputDTOBase { }

    [FunctionOutput]
    public class ViewLordBackerFundOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ViewRebelBackerFundOutputDTO : ViewRebelBackerFundOutputDTOBase { }

    [FunctionOutput]
    public class ViewRebelBackerFundOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ViewRebellionStatusOutputDTO : ViewRebellionStatusOutputDTOBase { }

    [FunctionOutput]
    public class ViewRebellionStatusOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class WarCasualtyRateOutputDTO : WarCasualtyRateOutputDTOBase { }

    [FunctionOutput]
    public class WarCasualtyRateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }


}
