import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  TodoListsClient,
  ClientVM, CreateClientCommand
} from '../web-api-client';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})
export class ClientComponent {
  debug = false;
  lists: ClientVM[];
  newClientEditor: any = {};
  newClientModalRef: BsModalRef;
  searchKey: string = '';

  constructor(
    private listsClient: TodoListsClient,
    private modalService: BsModalService
  ) { }

  ngOnInit(): void {
    this.getData('');
  }

  getData(searchKey: string) {
    this.listsClient.getClientLists(searchKey).subscribe(
      result => {
        this.lists = result;
      },
      error => console.error(error)
    );
  }

  showNewListModal(template: TemplateRef<any>): void {
    this.newClientModalRef = this.modalService.show(template);
    // setTimeout(() => document.getElementById('title').focus(), 250);
  }

  newListCancelled(): void {
    this.newClientModalRef.hide();
    this.newClientEditor = {};
  }

  addList(): void {

    console.log(this.newClientEditor);
    const list = {
      id: 0,
      firstName: this.newClientEditor.firstName,
      lastName: this.newClientEditor.lastName,
      mobileNumber: this.newClientEditor.mobileNumber,
      idNumber: this.newClientEditor.idNumber,
      physicalAddress: this.newClientEditor.physicalAddress,
    } as ClientVM;

    this.listsClient.createClient(list as CreateClientCommand).subscribe(
      result => {
        list.id = result;
        this.newClientEditor = {};
        this.getData('');
        this.newListCancelled();
      },
      error => {
        const errors = JSON.parse(error.response).errors;

        if (errors) {
          this.newClientEditor.error = errors;
          console.log(errors)
        }

      }
    );
  }

  onSearch() {
      this.getData(this.searchKey);
  }

}
