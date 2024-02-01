import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private timer: number;

  constructor(private _messageService: MessageService) {
    this._messageService.clear();
  }

  showMessage(messageType: string = 'info', clearFlag: boolean = true, detail: string, summary?: string): void {
    if (clearFlag) {
      this._messageService.clear();
    }
    if (summary.search('undefined')==-1 && detail.search('undefined')==-1) {
      this._messageService.add({ severity: messageType, summary: summary, detail: detail });
    }
  }

  showSuccess(detail: string, summary?: string): void {
    this._messageService.add({ severity: 'success', summary: summary, detail: detail });
  }

  showInfo(detail: string, summary?: string): void {
    this._messageService.add({ severity: 'info', summary: summary, detail: detail });
  }

  showWarning(detail: string, summary?: string): void {
    this._messageService.add({ severity: 'warn', summary: summary, detail: detail });
  }

  showError(detail: string, summary?: string): void {
    this._messageService.add({ severity: 'error', summary: summary, detail: detail });
  }
  
  showInfoForMilliseconds(message: string = '', ms: number = 3500) {
    this._messageService.clear();
    window.clearTimeout(this.timer);
    this._messageService.add({ severity: 'info', summary: message });
    this.timer = window.setTimeout(_ => this._messageService.clear(), ms);
  }

  showErrorForMilliseconds(message: string = '', ms: number = 3500) {
    this._messageService.clear();
    window.clearTimeout(this.timer);
    this._messageService.add({ severity: 'error', summary: message });
    this.timer = window.setTimeout(_ => this._messageService.clear(), ms);
  }
  showErrorForMillisecondsWithHeader(message: string = '', header: string = '',  ms: number = 3500) {
    this._messageService.clear();
    window.clearTimeout(this.timer);
    this._messageService.add({ severity: 'error', summary: header, detail: message });
    this.timer = window.setTimeout(_ => this._messageService.clear(), ms);
  }
  clearMessages() {
    this._messageService.clear();
  }
}
