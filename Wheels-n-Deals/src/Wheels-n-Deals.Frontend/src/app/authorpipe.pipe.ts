import { Pipe, PipeTransform } from '@angular/core';
import { User } from './Models/user';

@Pipe({
  name: 'authorpipe'
})
export class AuthorpipePipe implements PipeTransform {

  transform(value: User, ...args: unknown[]): string {
    return value.firstName + ' ' + value.lastName;
  }

}
