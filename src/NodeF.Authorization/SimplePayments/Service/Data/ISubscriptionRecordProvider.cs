﻿using NodeF.Fragments.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NodeF.Authorization.SimplePayments.Service.Data
{
    public interface ISubscriptionRecordProvider
    {
        Task<SubscriptionRecord> GetById(Guid userId);
        Task<IEnumerable<SubscriptionRecord>> GetAllById(Guid userId);
        Task Save(SubscriptionRecord record);
    }
}
