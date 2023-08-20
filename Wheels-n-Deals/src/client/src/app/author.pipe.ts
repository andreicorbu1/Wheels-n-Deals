import { Pipe, PipeTransform } from '@angular/core';
import { User } from './models/user';

@Pipe({
  name: 'author'
})
export class AuthorPipe implements PipeTransform {

  transform(value: User, ...args: unknown[]): unknown {
    return value.firstName + ' ' + value.lastName;
  }

}
