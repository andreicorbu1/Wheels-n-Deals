import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string | null = null;
  editMode: boolean = false;
  id: string;
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(10),
        ],
      ],
      address: [''],
    });

    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.editMode = true;
        this.id = params['id'];
        this.loadDataForEdit(params['id']);
      }
    });
  }

  loadDataForEdit(id: string) {
    this.userService.getUserById(id).subscribe((user) => {
      this.registerForm.patchValue({
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        phoneNumber: user.phoneNumber,
        address: user.address,
        password: '',
      });
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerData = this.registerForm.value;
      if (this.editMode) {
        this.userService.updateUser(this.id, registerData).subscribe({
          next: (response) => {
            this.router.navigate(['/profile']);
          },
          error: (error) => {
            console.error('Update failed:', error.error);
            this.errorMessage = error.error.message;
          },
        });
      } else {
        this.authService.register(registerData).subscribe({
          next: (response) => {
            this.router.navigate(['/login']);
          },
          error: (error) => {
            console.error('Registration failed:', error.error);
            this.errorMessage = error.error.message;
          },
        });
      }
    }
  }
}
