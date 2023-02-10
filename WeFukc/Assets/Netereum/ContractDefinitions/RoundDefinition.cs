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

namespace Contracts.Contracts.Round.ContractDefinition
{


    public partial class RoundDeployment : RoundDeploymentBase
    {
        public RoundDeployment() : base(BYTECODE) { }
        public RoundDeployment(string byteCode) : base(byteCode) { }
    }

    public class RoundDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x";
        public RoundDeploymentBase() : base(BYTECODE) { }
        public RoundDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address[13]", "_contracts", 1)]
        public virtual List<string> Contracts { get; set; }
        [Parameter("uint256", "_endOfTheFirstRound", 2)]
        public virtual BigInteger EndOfTheFirstRound { get; set; }
        [Parameter("uint256[10]", "_levelWeights", 3)]
        public virtual List<BigInteger> LevelWeights { get; set; }
        [Parameter("uint256", "_totalWeight", 4)]
        public virtual BigInteger TotalWeight { get; set; }
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

    public partial class ClaimBackerRewardFunction : ClaimBackerRewardFunctionBase { }

    [Function("claimBackerReward")]
    public class ClaimBackerRewardFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
    }

    public partial class ClaimPlayerRewardFunction : ClaimPlayerRewardFunctionBase { }

    [Function("claimPlayerReward")]
    public class ClaimPlayerRewardFunctionBase : FunctionMessage
    {
        [Parameter("bytes32[]", "_merkleProof", 1)]
        public virtual List<byte[]> MerkleProof { get; set; }
        [Parameter("uint256", "_roundNumber", 2)]
        public virtual BigInteger RoundNumber { get; set; }
    }

    public partial class ContractsFunction : ContractsFunctionBase { }

    [Function("contracts", "address")]
    public class ContractsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DefundBossFunction : DefundBossFunctionBase { }

    [Function("defundBoss", "bool")]
    public class DefundBossFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_levelNumber", 1)]
        public virtual BigInteger LevelNumber { get; set; }
        [Parameter("uint256", "_bossID", 2)]
        public virtual BigInteger BossID { get; set; }
        [Parameter("uint256", "_withdrawAmount", 3)]
        public virtual BigInteger WithdrawAmount { get; set; }
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

    public partial class FundBossFunction : FundBossFunctionBase { }

    [Function("fundBoss", "bool")]
    public class FundBossFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_levelNumber", 1)]
        public virtual BigInteger LevelNumber { get; set; }
        [Parameter("uint256", "_bossID", 2)]
        public virtual BigInteger BossID { get; set; }
        [Parameter("uint256", "_fundAmount", 3)]
        public virtual BigInteger FundAmount { get; set; }
    }

    public partial class GenesisCallFunction : GenesisCallFunctionBase { }

    [Function("genesisCall")]
    public class GenesisCallFunctionBase : FunctionMessage
    {

    }

    public partial class GetBackerRewardsFunction : GetBackerRewardsFunctionBase { }

    [Function("getBackerRewards", "uint256[10]")]
    public class GetBackerRewardsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
    }

    public partial class GetCurrentRoundNumberFunction : GetCurrentRoundNumberFunctionBase { }

    [Function("getCurrentRoundNumber", "uint256")]
    public class GetCurrentRoundNumberFunctionBase : FunctionMessage
    {

    }

    public partial class GetPlayerRewardsFunction : GetPlayerRewardsFunctionBase { }

    [Function("getPlayerRewards", "uint256[10]")]
    public class GetPlayerRewardsFunctionBase : FunctionMessage
    {
        [Parameter("bytes32[]", "_merkleProof", 1)]
        public virtual List<byte[]> MerkleProof { get; set; }
        [Parameter("uint256", "_roundNumber", 2)]
        public virtual BigInteger RoundNumber { get; set; }
    }

    public partial class IsCandidateFunction : IsCandidateFunctionBase { }

    [Function("isCandidate", "bool")]
    public class IsCandidateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
        [Parameter("uint256", "_levelNumber", 2)]
        public virtual BigInteger LevelNumber { get; set; }
        [Parameter("uint256", "_candidateID", 3)]
        public virtual BigInteger CandidateID { get; set; }
    }

    public partial class LevelRewardWeightsFunction : LevelRewardWeightsFunctionBase { }

    [Function("levelRewardWeights", "uint256")]
    public class LevelRewardWeightsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ProposalTypeIndexFunction : ProposalTypeIndexFunctionBase { }

    [Function("proposalTypeIndex", "uint256")]
    public class ProposalTypeIndexFunctionBase : FunctionMessage
    {

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
        [Parameter("uint256", "_newTypeIndex", 1)]
        public virtual BigInteger NewTypeIndex { get; set; }
    }

    public partial class ReturnMerkleRootFunction : ReturnMerkleRootFunctionBase { }

    [Function("returnMerkleRoot", "bytes32")]
    public class ReturnMerkleRootFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
        [Parameter("uint256", "_levelNumber", 2)]
        public virtual BigInteger LevelNumber { get; set; }
    }

    public partial class RoundCounterFunction : RoundCounterFunctionBase { }

    [Function("roundCounter", "uint256")]
    public class RoundCounterFunctionBase : FunctionMessage
    {

    }

    public partial class RoundLengthFunction : RoundLengthFunctionBase { }

    [Function("roundLength", "uint256")]
    public class RoundLengthFunctionBase : FunctionMessage
    {

    }

    public partial class RoundsFunction : RoundsFunctionBase { }

    [Function("rounds", typeof(RoundsOutputDTO))]
    public class RoundsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class SetPlayerMerkleRootAndNumberFunction : SetPlayerMerkleRootAndNumberFunctionBase { }

    [Function("setPlayerMerkleRootAndNumber")]
    public class SetPlayerMerkleRootAndNumberFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_round", 1)]
        public virtual BigInteger Round { get; set; }
        [Parameter("uint256", "_level", 2)]
        public virtual BigInteger Level { get; set; }
        [Parameter("bytes32", "_root", 3)]
        public virtual byte[] Root { get; set; }
        [Parameter("uint256", "_numberOfPlayers", 4)]
        public virtual BigInteger NumberOfPlayers { get; set; }
    }

    public partial class TotalRewardWeightFunction : TotalRewardWeightFunctionBase { }

    [Function("totalRewardWeight", "uint256")]
    public class TotalRewardWeightFunctionBase : FunctionMessage
    {

    }

    public partial class UpdateLevelRewardRatesFunction : UpdateLevelRewardRatesFunctionBase { }

    [Function("updateLevelRewardRates")]
    public class UpdateLevelRewardRatesFunctionBase : FunctionMessage
    {
        [Parameter("uint256[10]", "_newLevelWeights", 1)]
        public virtual List<BigInteger> NewLevelWeights { get; set; }
        [Parameter("uint256", "_newTotalWeight", 2)]
        public virtual BigInteger NewTotalWeight { get; set; }
    }

    public partial class ViewBackerFundsFunction : ViewBackerFundsFunctionBase { }

    [Function("viewBackerFunds", "uint256")]
    public class ViewBackerFundsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
        [Parameter("uint256", "_levelNumber", 2)]
        public virtual BigInteger LevelNumber { get; set; }
        [Parameter("uint256", "_candidateID", 3)]
        public virtual BigInteger CandidateID { get; set; }
        [Parameter("address", "_backer", 4)]
        public virtual string Backer { get; set; }
    }

    public partial class ViewCandidateFundsFunction : ViewCandidateFundsFunctionBase { }

    [Function("viewCandidateFunds", "uint256")]
    public class ViewCandidateFundsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
        [Parameter("uint256", "_levelNumber", 2)]
        public virtual BigInteger LevelNumber { get; set; }
        [Parameter("uint256", "_candidateID", 3)]
        public virtual BigInteger CandidateID { get; set; }
    }

    public partial class ViewElectionFunction : ViewElectionFunctionBase { }

    [Function("viewElection", typeof(ViewElectionOutputDTO))]
    public class ViewElectionFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
        [Parameter("uint256", "_levelNumber", 2)]
        public virtual BigInteger LevelNumber { get; set; }
    }

    public partial class ViewLevelFunction : ViewLevelFunctionBase { }

    [Function("viewLevel", typeof(ViewLevelOutputDTO))]
    public class ViewLevelFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_roundNumber", 1)]
        public virtual BigInteger RoundNumber { get; set; }
        [Parameter("uint256", "_levelNumber", 2)]
        public virtual BigInteger LevelNumber { get; set; }
    }

    public partial class ViewRoundNumberFunction : ViewRoundNumberFunctionBase { }

    [Function("viewRoundNumber", "uint256")]
    public class ViewRoundNumberFunctionBase : FunctionMessage
    {

    }









    public partial class ContractsOutputDTO : ContractsOutputDTOBase { }

    [FunctionOutput]
    public class ContractsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }











    public partial class GetBackerRewardsOutputDTO : GetBackerRewardsOutputDTOBase { }

    [FunctionOutput]
    public class GetBackerRewardsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[10]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }



    public partial class GetPlayerRewardsOutputDTO : GetPlayerRewardsOutputDTOBase { }

    [FunctionOutput]
    public class GetPlayerRewardsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[10]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }

    public partial class IsCandidateOutputDTO : IsCandidateOutputDTOBase { }

    [FunctionOutput]
    public class IsCandidateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class LevelRewardWeightsOutputDTO : LevelRewardWeightsOutputDTOBase { }

    [FunctionOutput]
    public class LevelRewardWeightsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ProposalTypeIndexOutputDTO : ProposalTypeIndexOutputDTOBase { }

    [FunctionOutput]
    public class ProposalTypeIndexOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
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





    public partial class ReturnMerkleRootOutputDTO : ReturnMerkleRootOutputDTOBase { }

    [FunctionOutput]
    public class ReturnMerkleRootOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class RoundCounterOutputDTO : RoundCounterOutputDTOBase { }

    [FunctionOutput]
    public class RoundCounterOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "_value", 1)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class RoundLengthOutputDTO : RoundLengthOutputDTOBase { }

    [FunctionOutput]
    public class RoundLengthOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class RoundsOutputDTO : RoundsOutputDTOBase { }

    [FunctionOutput]
    public class RoundsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "endingTime", 1)]
        public virtual BigInteger EndingTime { get; set; }
        [Parameter("uint256", "roundRewards", 2)]
        public virtual BigInteger RoundRewards { get; set; }
    }



    public partial class TotalRewardWeightOutputDTO : TotalRewardWeightOutputDTOBase { }

    [FunctionOutput]
    public class TotalRewardWeightOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class ViewBackerFundsOutputDTO : ViewBackerFundsOutputDTOBase { }

    [FunctionOutput]
    public class ViewBackerFundsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ViewCandidateFundsOutputDTO : ViewCandidateFundsOutputDTOBase { }

    [FunctionOutput]
    public class ViewCandidateFundsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ViewElectionOutputDTO : ViewElectionOutputDTOBase { }

    [FunctionOutput]
    public class ViewElectionOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class ViewLevelOutputDTO : ViewLevelOutputDTOBase { }

    [FunctionOutput]
    public class ViewLevelOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
        [Parameter("uint256", "", 3)]
        public virtual BigInteger ReturnValue3 { get; set; }
        [Parameter("bytes32", "", 4)]
        public virtual byte[] ReturnValue4 { get; set; }
    }

    public partial class ViewRoundNumberOutputDTO : ViewRoundNumberOutputDTOBase { }

    [FunctionOutput]
    public class ViewRoundNumberOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }
}
