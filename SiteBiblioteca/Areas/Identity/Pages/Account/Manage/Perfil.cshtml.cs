using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public class IndexModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public IndexModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public string Username { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        // Adicione as novas propriedades aqui
        [Display(Name = "Endereço")]
        public string Address { get; set; }

        [Display(Name = "Contato")]
        public string Contact { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Nome Completo")]
        public string FullName { get; set; }

        [Display(Name = "Email Confirmado")]
        public bool IsEmailConfirmed { get; set; }
    }

    private async Task LoadAsync(IdentityUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        // Aqui você deve também buscar os dados das novas propriedades
        // Se você tiver esses dados em algum lugar (por exemplo, no banco de dados), você deve buscá-los
        var email = await _userManager.GetEmailAsync(user);
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        Username = userName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber,
            Email = email,
            IsEmailConfirmed = isEmailConfirmed
            // Aqui você deve adicionar a lógica para carregar Address e Contact, se aplicável
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        // Aqui você deve adicionar lógica para atualizar as novas propriedades, se necessário
        // Por exemplo:
        // user.Address = Input.Address;
        // user.Contact = Input.Contact;
        // await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }
}
