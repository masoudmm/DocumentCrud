import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from '../../apiservice.service';

@Component({
  selector: 'app-show-Document',
  templateUrl: './show-Document.component.html',
  styleUrls: ['./show-Document.component.css']
})
export class ShowDocumentComponent implements OnInit {

  constructor(private service: ApiserviceService) { }

  DocumentList: any = [];
  ModalTitle = "";
  ActivateAddEditDocComp: boolean = false;
  doc: any;

  DocumentNumberFilter = "";
  DocumentExternalNumberFilter = "";
  DocumentListWithoutFilter: any = [];

  ngOnInit(): void {
    this.refreshDepList();
  }

  addClick() {
    this.doc = {
      DocumentNumber: "0",
      DocumentExternalNumber: ""
    }
    this.ModalTitle = "Add Document";
    this.ActivateAddEditDocComp = true;
  }

  editClick(item: any) {
    this.doc = item;
    this.ModalTitle = "Edit Document";
    this.ActivateAddEditDocComp = true;
  }

  deleteClick(item: any) {
    if (confirm('Are you sure??')) {
      this.service.deleteDocument(item.DocumentNumber).subscribe(data => {
        alert(data.toString());
        this.refreshDepList();
      })
    }
  }

  closeClick() {
    this.ActivateAddEditDocComp = false;
    this.refreshDepList();
  }


  refreshDepList() {
    this.service.getDocuments().subscribe(data => {
      this.DocumentList = data;
      this.DocumentListWithoutFilter = data;
    });
  }

  sortResult(prop: any, asc: any) {
    this.DocumentList = this.DocumentListWithoutFilter.sort(function (a: any, b: any) {
      if (asc) {
        return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
      }
      else {
        return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
      }
    });
  }

  FilterFn() {
    const DocumentNumberFilter = this.DocumentNumberFilter;
    const DocumentExternalNumberFilter = this.DocumentExternalNumberFilter;

    this.DocumentList = this.DocumentListWithoutFilter.filter(
      function (el: any) {
        return el.DocumentNumber.toString().toLowerCase().includes(
          DocumentNumberFilter.toString().trim().toLowerCase()
        ) &&
          el.DocumentExternalNumber.toString().toLowerCase().includes(
            DocumentExternalNumberFilter.toString().trim().toLowerCase())
      }
    );
  }
}
