import { Component, OnInit, Input } from '@angular/core';
import { ApiserviceService } from '../../apiservice.service';

@Component({
  selector: 'app-add-edit-document',
  templateUrl: './add-edit-document.component.html',
  styleUrls: ['./add-edit-document.component.css']
})
export class AddEditDocumentComponent implements OnInit {

  constructor(private service: ApiserviceService) { }

  @Input() doc: any;
  DocumentId = 0;
  DocumentNumber = "";
  DocumentExternalNumber = "";
  DocumentTotalAmount = "";
  DocumentStatus = "";

  ngOnInit(): void {

    this.DocumentId = this.doc.Id;
    this.DocumentNumber = this.doc.DocumentNumber;
    this.DocumentExternalNumber = this.doc.DocumentExternalNumber;
    this.DocumentTotalAmount = this.doc.DocumentTotalAmount;
    this.DocumentStatus = this.doc.DocumentStatus;
  }

  addDocument() {
    var doc = {
      DocumentNumber: this.DocumentNumber,
      DocumentExternalNumber: this.DocumentExternalNumber,
      DocumentTotalAmount: this.DocumentTotalAmount
    };
    this.service.addDocument(doc).subscribe(res => {
      alert(res.toString());
    });
  }

  editDocument() {
    var doc = {
      DocumentNumber: this.DocumentNumber,
      DocumentExternalNumber: this.DocumentExternalNumber,
      DocumentTotalAmount: this.DocumentTotalAmount,
      DocumentStatus: this.DocumentStatus
    };
    this.service.editDocument(doc).subscribe(res => {
      alert(res.toString());
    });
  }
}
