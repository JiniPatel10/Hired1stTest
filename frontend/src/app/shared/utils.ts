import { FormArray, FormControl, FormGroup } from "@angular/forms";

export class Utils { 
    static validateAllFormFields(formGroup: FormGroup) {
        Object.keys(formGroup.controls).forEach(field => {
          let control = formGroup.get(field);
          if (control instanceof FormControl) {
            control.markAsTouched({ onlySelf: true });
            control.markAsDirty({ onlySelf: true });
          } else if (control instanceof FormGroup) {
            this.validateAllFormFields(control);
          }else if (control instanceof FormArray) {
            control.controls.forEach(item => {
               this.validateAllFormFields(item as FormGroup);
            });
          }
        });
      }

}