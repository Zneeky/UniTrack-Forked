import { CanActivateFn } from '@angular/router';
import { UserService } from '../user.service';
import { inject } from '@angular/core';
import { Roles } from '../../enums/roles.enum';

export const adminGuard: CanActivateFn = (route, state) => {
  const userService: UserService = inject(UserService);
  return userService.getRole() === Roles.ADMIN;
};
