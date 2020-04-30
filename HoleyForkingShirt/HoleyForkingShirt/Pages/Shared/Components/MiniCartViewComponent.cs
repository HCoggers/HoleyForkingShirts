using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Views.Shared.Components
{
    [ViewComponent, Authorize]
    public class MiniCartViewComponent : ViewComponent
    {
        private ICartManager _cartManager;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public MiniCartViewComponent(ICartManager cartManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _cartManager = cartManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var signedIn = _signInManager.IsSignedIn(UserClaimsPrincipal);
            if (signedIn)
            {
                var userId = _userManager.GetUserId(UserClaimsPrincipal);
                Cart cart = await _cartManager.GetCart(userId);
                List<CartItems> items = await _cartManager.GetAllItems(cart.ID);

                return View(items);
            }
            else
            {
                return View();
            }

        }
    }
}
