using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DSHI_diplom.Model;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле фамилии обязательно для заполнения")]
    [StringLength(15, ErrorMessage = "Фамилия должна содержать не более 15 символов")]
    [RegularExpression(@"^[А-ЯЁ][а-яё]*$", ErrorMessage = "Проверьте корректность поля")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Поле имени обязательно для заполнения")]
    [StringLength(15, ErrorMessage = "Имя должно содержать не более 15 символов")]
    [RegularExpression(@"^[А-ЯЁ][а-яё]*$", ErrorMessage = "Проверьте корректность поля")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле отчества обязательно для заполнения")]
    [StringLength(15, ErrorMessage = "Отчество должно содержать не более 15 символов")]
    [RegularExpression(@"^[А-ЯЁ][а-яё]*$", ErrorMessage = "Проверьте корректность поля")]
    public string MiddleName { get; set; } = null!;

    [Required(ErrorMessage = "Поле логина обязательно для заполнения")]
    [StringLength(10, ErrorMessage = "Логин должен содержать не более 10 символов")]
    [RegularExpression(@"^[a-z0-9_.]+$", ErrorMessage = "Логин должен содержать только латинские буквы, цифры, символы _ и .")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Поле пароля обязательно для заполнения")]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Пароль должен содержать не менее 6 и не более 15 символов")]
    [RegularExpression(@"^[^\p{IsCyrillic}]*$", ErrorMessage = "Проверьте корректность поля")]
    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<CollectionOfNote> CollectionOfNotes { get; set; } = new List<CollectionOfNote>();

    public virtual ICollection<CollectionOfTheoreticalMaterial> CollectionOfTheoreticalMaterials { get; set; } = new List<CollectionOfTheoreticalMaterial>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
