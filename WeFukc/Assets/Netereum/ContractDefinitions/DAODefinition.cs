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

namespace Contracts.Contracts.DAO.ContractDefinition
{


    public partial class DAODeployment : DAODeploymentBase
    {
        public DAODeployment() : base(BYTECODE) { }
        public DAODeployment(string byteCode) : base(byteCode) { }
    }

    public class DAODeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x";
        public DAODeploymentBase() : base(BYTECODE) { }
        public DAODeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address[13]", "_contracts", 1)]
        public virtual List<string> Contracts { get; set; }
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

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2)]
        public virtual string Spender { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class ClaimCoinSpendingFunction : ClaimCoinSpendingFunctionBase { }

    [Function("claimCoinSpending")]
    public class ClaimCoinSpendingFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_spendingProposalNumber", 1)]
        public virtual BigInteger SpendingProposalNumber { get; set; }
        [Parameter("bytes32[]", "_merkleProof", 2)]
        public virtual List<byte[]> MerkleProof { get; set; }
    }

    public partial class ClaimTokenSpendingFunction : ClaimTokenSpendingFunctionBase { }

    [Function("claimTokenSpending")]
    public class ClaimTokenSpendingFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_spendingProposalNumber", 1)]
        public virtual BigInteger SpendingProposalNumber { get; set; }
        [Parameter("bytes32[]", "_merkleProof", 2)]
        public virtual List<byte[]> MerkleProof { get; set; }
    }

    public partial class ContractsFunction : ContractsFunctionBase { }

    [Function("contracts", "address")]
    public class ContractsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint8")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class DecreaseAllowanceFunction : DecreaseAllowanceFunctionBase { }

    [Function("decreaseAllowance", "bool")]
    public class DecreaseAllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "subtractedValue", 2)]
        public virtual BigInteger SubtractedValue { get; set; }
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

    public partial class ExecuteMinBalanceToPropUpdateProposalFunction : ExecuteMinBalanceToPropUpdateProposalFunctionBase { }

    [Function("executeMinBalanceToPropUpdateProposal")]
    public class ExecuteMinBalanceToPropUpdateProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteNewProposalTypeProposalFunction : ExecuteNewProposalTypeProposalFunctionBase { }

    [Function("executeNewProposalTypeProposal")]
    public class ExecuteNewProposalTypeProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ExecuteProposalTypeUpdateProposalFunction : ExecuteProposalTypeUpdateProposalFunctionBase { }

    [Function("executeProposalTypeUpdateProposal")]
    public class ExecuteProposalTypeUpdateProposalFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class FinalizeSpendingProposalFunction : FinalizeSpendingProposalFunctionBase { }

    [Function("finalizeSpendingProposal")]
    public class FinalizeSpendingProposalFunctionBase : FunctionMessage
    {

    }

    public partial class FunctionsProposalTypesFunction : FunctionsProposalTypesFunctionBase { }

    [Function("functionsProposalTypes", "uint256")]
    public class FunctionsProposalTypesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetContractCoinBalanceFunction : GetContractCoinBalanceFunctionBase { }

    [Function("getContractCoinBalance", "uint256")]
    public class GetContractCoinBalanceFunctionBase : FunctionMessage
    {

    }

    public partial class GetContractTokenBalanceFunction : GetContractTokenBalanceFunctionBase { }

    [Function("getContractTokenBalance", "uint256")]
    public class GetContractTokenBalanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "_tokenContractAddress", 1)]
        public virtual string TokenContractAddress { get; set; }
    }

    public partial class GetMinBalanceToProposeFunction : GetMinBalanceToProposeFunctionBase { }

    [Function("getMinBalanceToPropose", "uint256")]
    public class GetMinBalanceToProposeFunctionBase : FunctionMessage
    {

    }

    public partial class IncreaseAllowanceFunction : IncreaseAllowanceFunctionBase { }

    [Function("increaseAllowance", "bool")]
    public class IncreaseAllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "addedValue", 2)]
        public virtual BigInteger AddedValue { get; set; }
    }

    public partial class IsProposalPassedFunction : IsProposalPassedFunctionBase { }

    [Function("isProposalPassed", "bool")]
    public class IsProposalPassedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class LordVoteFunction : LordVoteFunctionBase { }

    [Function("lordVote", "string")]
    public class LordVoteFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
        [Parameter("bool", "_isApproving", 2)]
        public virtual bool IsApproving { get; set; }
        [Parameter("uint256", "_lordID", 3)]
        public virtual BigInteger LordID { get; set; }
        [Parameter("uint256", "_lordTotalSupply", 4)]
        public virtual BigInteger LordTotalSupply { get; set; }
    }

    public partial class MinBalanceToProposeFunction : MinBalanceToProposeFunctionBase { }

    [Function("minBalanceToPropose", "uint256")]
    public class MinBalanceToProposeFunctionBase : FunctionMessage
    {

    }

    public partial class MintTokensFunction : MintTokensFunctionBase { }

    [Function("mintTokens")]
    public class MintTokensFunctionBase : FunctionMessage
    {
        [Parameter("address", "_minter", 1)]
        public virtual string Minter { get; set; }
        [Parameter("uint256", "_amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class NewProposalFunction : NewProposalFunctionBase { }

    [Function("newProposal", "uint256")]
    public class NewProposalFunctionBase : FunctionMessage
    {
        [Parameter("string", "_description", 1)]
        public virtual string Description { get; set; }
        [Parameter("uint256", "_proposalType", 2)]
        public virtual BigInteger ProposalType { get; set; }
    }

    public partial class ProposalResultFunction : ProposalResultFunctionBase { }

    [Function("proposalResult", "uint256")]
    public class ProposalResultFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
    }

    public partial class ProposalTrackersFunction : ProposalTrackersFunctionBase { }

    [Function("proposalTrackers", typeof(ProposalTrackersOutputDTO))]
    public class ProposalTrackersFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ProposalTypeUpdatesFunction : ProposalTypeUpdatesFunctionBase { }

    [Function("proposalTypeUpdates", typeof(ProposalTypeUpdatesOutputDTO))]
    public class ProposalTypeUpdatesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ProposalTypesFunction : ProposalTypesFunctionBase { }

    [Function("proposalTypes", typeof(ProposalTypesOutputDTO))]
    public class ProposalTypesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ProposalsFunction : ProposalsFunctionBase { }

    [Function("proposals", typeof(ProposalsOutputDTO))]
    public class ProposalsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
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

    public partial class ProposeMinBalanceToPropUpdateFunction : ProposeMinBalanceToPropUpdateFunctionBase { }

    [Function("proposeMinBalanceToPropUpdate")]
    public class ProposeMinBalanceToPropUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_newAmount", 1)]
        public virtual BigInteger NewAmount { get; set; }
    }

    public partial class ProposeNewCoinSpendingFunction : ProposeNewCoinSpendingFunctionBase { }

    [Function("proposeNewCoinSpending")]
    public class ProposeNewCoinSpendingFunctionBase : FunctionMessage
    {
        [Parameter("bytes32[]", "_merkleRoots", 1)]
        public virtual List<byte[]> MerkleRoots { get; set; }
        [Parameter("uint256[]", "_allowances", 2)]
        public virtual List<BigInteger> Allowances { get; set; }
        [Parameter("uint256", "_totalSpending", 3)]
        public virtual BigInteger TotalSpending { get; set; }
    }

    public partial class ProposeNewProposalTypeFunction : ProposeNewProposalTypeFunctionBase { }

    [Function("proposeNewProposalType")]
    public class ProposeNewProposalTypeFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_length", 1)]
        public virtual BigInteger Length { get; set; }
        [Parameter("uint256", "_requiredApprovalRate", 2)]
        public virtual BigInteger RequiredApprovalRate { get; set; }
        [Parameter("uint256", "_requiredTokenAmount", 3)]
        public virtual BigInteger RequiredTokenAmount { get; set; }
        [Parameter("uint256", "_requiredParticipantAmount", 4)]
        public virtual BigInteger RequiredParticipantAmount { get; set; }
    }

    public partial class ProposeNewTokenSpendingFunction : ProposeNewTokenSpendingFunctionBase { }

    [Function("proposeNewTokenSpending")]
    public class ProposeNewTokenSpendingFunctionBase : FunctionMessage
    {
        [Parameter("address", "_tokenContractAddress", 1)]
        public virtual string TokenContractAddress { get; set; }
        [Parameter("bytes32[]", "_merkleRoots", 2)]
        public virtual List<byte[]> MerkleRoots { get; set; }
        [Parameter("uint256[]", "_allowances", 3)]
        public virtual List<BigInteger> Allowances { get; set; }
        [Parameter("uint256", "_totalSpending", 4)]
        public virtual BigInteger TotalSpending { get; set; }
    }

    public partial class ProposeProposalTypeUpdateFunction : ProposeProposalTypeUpdateFunctionBase { }

    [Function("proposeProposalTypeUpdate")]
    public class ProposeProposalTypeUpdateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalTypeNumber", 1)]
        public virtual BigInteger ProposalTypeNumber { get; set; }
        [Parameter("uint256", "_newLength", 2)]
        public virtual BigInteger NewLength { get; set; }
        [Parameter("uint256", "_newRequiredApprovalRate", 3)]
        public virtual BigInteger NewRequiredApprovalRate { get; set; }
        [Parameter("uint256", "_newRequiredTokenAmount", 4)]
        public virtual BigInteger NewRequiredTokenAmount { get; set; }
        [Parameter("uint256", "_newRequiredParticipantAmount", 5)]
        public virtual BigInteger NewRequiredParticipantAmount { get; set; }
    }

    public partial class ReturnAllowancesFunction : ReturnAllowancesFunctionBase { }

    [Function("returnAllowances", "uint256[]")]
    public class ReturnAllowancesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_spendingProposalNumber", 1)]
        public virtual BigInteger SpendingProposalNumber { get; set; }
    }

    public partial class ReturnMerkleRootsFunction : ReturnMerkleRootsFunctionBase { }

    [Function("returnMerkleRoots", "bytes32[]")]
    public class ReturnMerkleRootsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_spendingProposalNumber", 1)]
        public virtual BigInteger SpendingProposalNumber { get; set; }
    }

    public partial class SpendingProposalsFunction : SpendingProposalsFunctionBase { }

    [Function("spendingProposals", typeof(SpendingProposalsOutputDTO))]
    public class SpendingProposalsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom", "bool")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class VoteFunction : VoteFunctionBase { }

    [Function("vote")]
    public class VoteFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_proposalID", 1)]
        public virtual BigInteger ProposalID { get; set; }
        [Parameter("bool", "_isApproving", 2)]
        public virtual bool IsApproving { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2, true )]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }





    public partial class AllowanceOutputDTO : AllowanceOutputDTOBase { }

    [FunctionOutput]
    public class AllowanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
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

    public partial class DecimalsOutputDTO : DecimalsOutputDTOBase { }

    [FunctionOutput]
    public class DecimalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "", 1)]
        public virtual byte ReturnValue1 { get; set; }
    }















    public partial class FunctionsProposalTypesOutputDTO : FunctionsProposalTypesOutputDTOBase { }

    [FunctionOutput]
    public class FunctionsProposalTypesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetContractCoinBalanceOutputDTO : GetContractCoinBalanceOutputDTOBase { }

    [FunctionOutput]
    public class GetContractCoinBalanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetContractTokenBalanceOutputDTO : GetContractTokenBalanceOutputDTOBase { }

    [FunctionOutput]
    public class GetContractTokenBalanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetMinBalanceToProposeOutputDTO : GetMinBalanceToProposeOutputDTOBase { }

    [FunctionOutput]
    public class GetMinBalanceToProposeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }







    public partial class MinBalanceToProposeOutputDTO : MinBalanceToProposeOutputDTOBase { }

    [FunctionOutput]
    public class MinBalanceToProposeOutputDTOBase : IFunctionOutputDTO 
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





    public partial class ProposalTrackersOutputDTO : ProposalTrackersOutputDTOBase { }

    [FunctionOutput]
    public class ProposalTrackersOutputDTOBase : IFunctionOutputDTO 
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

    public partial class ProposalTypeUpdatesOutputDTO : ProposalTypeUpdatesOutputDTOBase { }

    [FunctionOutput]
    public class ProposalTypeUpdatesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "status", 1)]
        public virtual byte Status { get; set; }
        [Parameter("bool", "isExecuted", 2)]
        public virtual bool IsExecuted { get; set; }
        [Parameter("bool", "isNewType", 3)]
        public virtual bool IsNewType { get; set; }
        [Parameter("uint256", "proposalTypeNumber", 4)]
        public virtual BigInteger ProposalTypeNumber { get; set; }
        [Parameter("uint256", "newLength", 5)]
        public virtual BigInteger NewLength { get; set; }
        [Parameter("uint256", "newRequiredApprovalRate", 6)]
        public virtual BigInteger NewRequiredApprovalRate { get; set; }
        [Parameter("uint256", "newRequiredTokenAmount", 7)]
        public virtual BigInteger NewRequiredTokenAmount { get; set; }
        [Parameter("uint256", "newRequiredParticipantAmount", 8)]
        public virtual BigInteger NewRequiredParticipantAmount { get; set; }
    }

    public partial class ProposalTypesOutputDTO : ProposalTypesOutputDTOBase { }

    [FunctionOutput]
    public class ProposalTypesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "length", 1)]
        public virtual BigInteger Length { get; set; }
        [Parameter("uint256", "requiredApprovalRate", 2)]
        public virtual BigInteger RequiredApprovalRate { get; set; }
        [Parameter("uint256", "requiredTokenAmount", 3)]
        public virtual BigInteger RequiredTokenAmount { get; set; }
        [Parameter("uint256", "requiredParticipantAmount", 4)]
        public virtual BigInteger RequiredParticipantAmount { get; set; }
    }

    public partial class ProposalsOutputDTO : ProposalsOutputDTOBase { }

    [FunctionOutput]
    public class ProposalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("string", "description", 2)]
        public virtual string Description { get; set; }
        [Parameter("uint256", "startTime", 3)]
        public virtual BigInteger StartTime { get; set; }
        [Parameter("uint256", "proposalType", 4)]
        public virtual BigInteger ProposalType { get; set; }
        [Parameter("uint8", "status", 5)]
        public virtual byte Status { get; set; }
        [Parameter("uint256", "participants", 6)]
        public virtual BigInteger Participants { get; set; }
        [Parameter("uint256", "totalVotes", 7)]
        public virtual BigInteger TotalVotes { get; set; }
        [Parameter("uint256", "yayCount", 8)]
        public virtual BigInteger YayCount { get; set; }
        [Parameter("uint256", "nayCount", 9)]
        public virtual BigInteger NayCount { get; set; }
    }















    public partial class ReturnAllowancesOutputDTO : ReturnAllowancesOutputDTOBase { }

    [FunctionOutput]
    public class ReturnAllowancesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }

    public partial class ReturnMerkleRootsOutputDTO : ReturnMerkleRootsOutputDTOBase { }

    [FunctionOutput]
    public class ReturnMerkleRootsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32[]", "", 1)]
        public virtual List<byte[]> ReturnValue1 { get; set; }
    }

    public partial class SpendingProposalsOutputDTO : SpendingProposalsOutputDTOBase { }

    [FunctionOutput]
    public class SpendingProposalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "status", 1)]
        public virtual byte Status { get; set; }
        [Parameter("uint256", "proposalID", 2)]
        public virtual BigInteger ProposalID { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("address", "tokenAddress", 4)]
        public virtual string TokenAddress { get; set; }
        [Parameter("uint256", "totalClaimedAmount", 5)]
        public virtual BigInteger TotalClaimedAmount { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
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






}
