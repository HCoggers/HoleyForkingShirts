﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models.Interfaces
{
    public interface ICartManager
    {
        // CREATE
        public Task<Cart> CreateCart(Cart cart);

        // READ
        public Task<Cart> GetCart(string email);

        // UPDATE
        public Task UpdateCart(int id, Cart cart);

        // DELETE
        public Task DeleteCart(int id);
    }
}