using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Linq.Expressions;
using XOracle.Data.Azure.Entities;
using XOracle.Domain;
using XOracle.Domain.Core;

namespace XOracle.Data.Azure
{
    public static class AzureEntityFactory
    {
        public const string SELF_ID_PARTIOTION_FORMAT = "##e0";

        public static TAzureEntity ToAzureEntity<TAzureEntity, TEntity>(TEntity entity)
            where TAzureEntity : TableServiceEntity
            where TEntity : Entity
        {
            if (entity is Account)
            {
                var e = entity as Account;
                return new AzureAccount
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email
                } as TAzureEntity;
            }
            else if (entity is AccountBalance)
            {
                var e = entity as AccountBalance;
                return new AzureAccountBalance
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    CurrencyTypeId = e.CurrencyTypeId,
                    Value = e.Value
                } as TAzureEntity;
            }
            else if (entity is AccountLogin)
            {
                var e = entity as AccountLogin;
                return new AzureAccountLogin
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    LoginProvider = e.LoginProvider,
                    ProviderKey = e.ProviderKey
                } as TAzureEntity;
            }
            else if (entity is AccountSet)
            {
                var e = entity as AccountSet;
                return new AzureAccountSet
                {
                    Id = e.Id,
                    AccountId = e.AccountId
                } as TAzureEntity;
            }
            else if (entity is AccountSetAccounts)
            {
                var e = entity as AccountSetAccounts;
                return new AzureAccountSetAccounts
                {
                    Id = e.Id,
                    AccountSetId = e.AccountSetId,
                    AccountId = e.AccountId
                } as TAzureEntity;
            }
            else if (entity is Bet)
            {
                var e = entity as Bet;
                return new AzureBet
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    CreationDate = e.CreationDate,
                    CurrencyTypeId = e.CurrencyTypeId,
                    EventId = e.EventId,
                    OutcomesTypeId = e.OutcomesTypeId,
                    Value = e.Value
                } as TAzureEntity;
            }
            else if (entity is BetRateAlgorithm)
            {
                var e = entity as BetRateAlgorithm;
                return new AzureBetRateAlgorithm
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    AlgorithmTypeId = e.AlgorithmTypeId,
                    EndRate = e.EndRate,
                    StartRate = e.StartRate,
                    LocusRage = e.LocusRage
                } as TAzureEntity;
            }
            else if (entity is AlgorithmType)
            {
                var e = entity as AlgorithmType;
                return new AzureAlgorithmType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TAzureEntity;
            }
            else if (entity is CurrencyType)
            {
                var e = entity as CurrencyType;
                return new AzureCurrencyType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TAzureEntity;
            }
            else if (entity is OutcomesType)
            {
                var e = entity as OutcomesType;
                return new AzureOutcomesType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TAzureEntity;
            }
            else if (entity is Event)
            {
                var e = entity as Event;
                return new AzureEvent
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    EndDate = e.EndDate,
                    EventBetConditionId = e.EventBetConditionId,
                    EventRelationTypeId = e.EventRelationTypeId,
                    ExpectedEventConditionId = e.ExpectedEventConditionId,
                    ImageId = e.ImageId,
                    JudgingAccountSetId = e.JudgingAccountSetId,
                    ParticipantsAccountSetId = e.ParticipantsAccountSetId,
                    RealEventConditionId = e.RealEventConditionId,
                    StartDate = e.StartDate,
                    Title = e.Title
                } as TAzureEntity;
            }
            else if (entity is EventBetCondition)
            {
                var e = entity as EventBetCondition;
                return new AzureEventBetCondition
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    CloseDate = e.CloseDate,
                    CurrencyTypeId = e.CurrencyTypeId,
                    EventBetRateAlgorithmId = e.EventBetRateAlgorithmId
                } as TAzureEntity;
            }
            else if (entity is EventCondition)
            {
                var e = entity as EventCondition;
                return new AzureEventCondition
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    Description = e.Description
                } as TAzureEntity;
            }
            else if (entity is EventRelationType)
            {
                var e = entity as EventRelationType;
                return new AzureEventRelationType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TAzureEntity;
            }

            return null;
        }

        public static TEntity FromAzureEntity<TAzureEntity, TEntity>(TAzureEntity entity)
            where TAzureEntity : TableServiceEntity
            where TEntity : Entity
        {
            if (entity is AzureAccount)
            {
                var e = entity as AzureAccount;
                return new Account
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email
                } as TEntity;
            }
            else if (entity is AzureAccountBalance)
            {
                var e = entity as AzureAccountBalance;
                return new AccountBalance
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    CurrencyTypeId = e.CurrencyTypeId,
                    Value = e.Value
                } as TEntity;
            }
            else if (entity is AzureAccountLogin)
            {
                var e = entity as AccountLogin;
                return new AccountLogin
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    LoginProvider = e.LoginProvider,
                    ProviderKey = e.ProviderKey
                } as TEntity;
            }
            else if (entity is AzureAccountSet)
            {
                var e = entity as AccountSet;
                return new AccountSet
                {
                    Id = e.Id,
                    AccountId = e.AccountId
                } as TEntity;
            }
            else if (entity is AzureAccountSetAccounts)
            {
                var e = entity as AccountSetAccounts;
                return new AccountSetAccounts
                {
                    Id = e.Id,
                    AccountSetId = e.AccountSetId,
                    AccountId = e.AccountId
                } as TEntity;
            }
            else if (entity is AzureBet)
            {
                var e = entity as Bet;
                return new Bet
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    CreationDate = e.CreationDate,
                    CurrencyTypeId = e.CurrencyTypeId,
                    EventId = e.EventId,
                    OutcomesTypeId = e.OutcomesTypeId,
                    Value = e.Value
                } as TEntity;
            }
            else if (entity is AzureBetRateAlgorithm)
            {
                var e = entity as BetRateAlgorithm;
                return new BetRateAlgorithm
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    AlgorithmTypeId = e.AlgorithmTypeId,
                    EndRate = e.EndRate,
                    StartRate = e.StartRate,
                    LocusRage = e.LocusRage
                } as TEntity;
            }
            else if (entity is AzureAlgorithmType)
            {
                var e = entity as AlgorithmType;
                return new AlgorithmType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TEntity;
            }
            else if (entity is AzureCurrencyType)
            {
                var e = entity as CurrencyType;
                return new CurrencyType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TEntity;
            }
            else if (entity is AzureOutcomesType)
            {
                var e = entity as OutcomesType;
                return new OutcomesType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TEntity;
            }
            else if (entity is AzureEvent)
            {
                var e = entity as Event;
                return new Event
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    EndDate = e.EndDate,
                    EventBetConditionId = e.EventBetConditionId,
                    EventRelationTypeId = e.EventRelationTypeId,
                    ExpectedEventConditionId = e.ExpectedEventConditionId,
                    ImageId = e.ImageId,
                    JudgingAccountSetId = e.JudgingAccountSetId,
                    ParticipantsAccountSetId = e.ParticipantsAccountSetId,
                    RealEventConditionId = e.RealEventConditionId,
                    StartDate = e.StartDate,
                    Title = e.Title
                } as TEntity;
            }
            else if (entity is AzureEventBetCondition)
            {
                var e = entity as EventBetCondition;
                return new EventBetCondition
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    CloseDate = e.CloseDate,
                    CurrencyTypeId = e.CurrencyTypeId,
                    EventBetRateAlgorithmId = e.EventBetRateAlgorithmId
                } as TEntity;
            }
            else if (entity is AzureEventCondition)
            {
                var e = entity as EventCondition;
                return new EventCondition
                {
                    Id = e.Id,
                    AccountId = e.AccountId,
                    Description = e.Description
                } as TEntity;
            }
            else if (entity is AzureEventRelationType)
            {
                var e = entity as EventRelationType;
                return new EventRelationType
                {
                    Id = e.Id,
                    Name = e.Name
                } as TEntity;
            }

            return null;
        }

        public static Expression<Func<TAzureEntity, bool>> ToAzureFilter<TAzureEntity, TEntity>(Expression<Func<TEntity, bool>> filter)
            where TAzureEntity : TableServiceEntity
            where TEntity : Entity
        {
            return AzureTransferExpression<TAzureEntity, TEntity>.Transfer(filter);
        }
    }
}
