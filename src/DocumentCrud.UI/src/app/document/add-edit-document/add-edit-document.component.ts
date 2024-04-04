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
  DocumentNumber = "";
  DocumentExternalNumber = "";

  ngOnInit(): void {

    this.DocumentNumber = this.doc.DocumentNumber;
    this.DocumentExternalNumber = this.doc.DocumentExternalNumber;
  }

  addDocument() {
    var doc = {
      DocumentNumber: this.DocumentNumber,
      DocumentExternalNumber: this.DocumentExternalNumber
    };
    this.service.addDocument(doc).subscribe(res => {
      alert(res.toString());
    });
  }

  editDocument() {
    var doc = {
      DocumentNumber: this.DocumentNumber,
      DocumentExternalNumber: this.DocumentExternalNumber
    };
    this.service.editDocument(doc).subscribe(res => {
      alert(res.toString());
    });
  }
}
