using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PizzaShop.Tests.Classes
{
    class ModelValidator<T> where T : class
    {
        ValidationContext _context;
        List<ValidationResult> _results;
        T _model;

        public ModelValidator(T model)
        {
            _context = new ValidationContext(model, null, null);
            _results = new List<ValidationResult>();
            _model = model;
        }

        public bool IsValid()
        {
            var result = Validator.TryValidateObject(_model, _context, _results, true);
            return result;
        }

        public void AddToModelError(Controller controller)
        {
            foreach (var validationResult in _results)
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
        }
    }
}
