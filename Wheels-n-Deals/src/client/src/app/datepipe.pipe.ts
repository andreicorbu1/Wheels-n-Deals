import { formatDate } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'datepipe'
})
export class DatepipePipe implements PipeTransform {

  transform(value: Date, ...args: unknown[]): unknown {
    return formatDate(value, 'yyyy-MM-dd HH:mm:ss', 'en-US', '+0300');
  }

}
