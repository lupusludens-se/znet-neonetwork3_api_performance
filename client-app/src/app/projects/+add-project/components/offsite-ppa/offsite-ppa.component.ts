import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as MapboxGeocoder from '@mapbox/mapbox-gl-geocoder';
import { Component, OnInit } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';

import { ProjectOffsitePpaInterface } from '../../../../shared/interfaces/projects/project-creation.interface';
import { CountryInterface } from '../../../../shared/interfaces/user/country.interface';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { environment } from '../../../../../environments/environment';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { HttpService } from '../../../../core/services/http.service';
import { INT32_MAX } from 'src/app/shared/constants/math.const';

@Component({
  selector: 'neo-offsite-ppa',
  templateUrl: './offsite-ppa.component.html',
  styleUrls: ['../../add-project.component.scss', './offsite-ppa.component.scss']
})
export class OffsitePpaComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  isoList: TagInterface[] = [
    { name: 'None', id: null },
    { name: 'PJM', id: 1 },
    { name: 'ISO-NE', id: 2 },
    { name: 'MISO', id: 3 },
    { name: 'SERC', id: 4 },
    { name: 'SPP', id: 5 },
    { name: 'ERCOT', id: 6 },
    { name: 'WECC', id: 7 },
    { name: 'CAISO', id: 8 },
    { name: 'AESO', id: 9 },
    { name: 'NYISO', id: 10 },
    { name: 'Other', id: 11 }
  ];
  projectTypeList: TagInterface[] = [
    { name: 'Energy only', id: 1 },
    { name: 'Energy with project EACs', id: 2 },
    { name: 'Energy with certified swap EACs', id: 3 },
    { name: 'Retail delivered product', id: 4 }
  ];
  offtakerValueList: TagInterface[] = [
    { name: 'Renewable Attributes', id: 6 },
    { name: 'Energy Arbitrage', id: 7 },
    { name: 'Visible Commitment to Climate Action', id: 8 },
    { name: 'Community Benefits', id: 9 }
  ];
  form: FormGroup = this.formBuilder.group({
    latitude: ['', Validators.required],
    longitude: ['', Validators.required],
    isoRto: [''],
    productType: ['', Validators.required],
    commercialOperationDate: ['', Validators.required],
    valuesToOfftakers: [[], Validators.required],
    ppaTermYearsLength: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    totalProjectNameplateMWACCapacity: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    totalProjectExpectedAnnualMWhProductionP50: [
      '',
      [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]
    ],
    minimumOfftakeMWhVolumeRequired: ['', [Validators.required, Validators.min(1), Validators.max(INT32_MAX)]],
    notesForPotentialOfftakers: ['', Validators.maxLength(500)]
  });
  minDate: Date;
  map: mapboxgl.Map;
  isoRtoId: number;
  productTypeId: number;
  geocoder: MapboxGeocoder;
  showDraftModal: boolean;
  formSubmitted: boolean;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private formBuilder: FormBuilder,
    private readonly httpService: HttpService
  ) {}

  ngOnInit(): void {
    this.setupMap();

    if (this.addProjectService.currentFlowData?.projectDetails) {
      const formVal: ProjectOffsitePpaInterface = this.addProjectService.currentFlowData
        ?.projectDetails as ProjectOffsitePpaInterface;

      this.form.patchValue({
        latitude: formVal.latitude,
        longitude: formVal.longitude,
        commercialOperationDate: formVal.commercialOperationDate,
        valuesToOfftakers: formVal.valuesToOfftakers,
        ppaTermYearsLength: formVal.ppaTermYearsLength,
        totalProjectNameplateMWACCapacity: formVal.totalProjectNameplateMWACCapacity,
        totalProjectExpectedAnnualMWhProductionP50: formVal.totalProjectExpectedAnnualMWhProductionP50,
        minimumOfftakeMWhVolumeRequired: formVal.minimumOfftakeMWhVolumeRequired,
        notesForPotentialOfftakers: formVal.notesForPotentialOfftakers
      });

      if (formVal.isoRtoId) {
        this.form.patchValue({
          isoRto: this.isoList.filter(i => i.id === formVal.isoRtoId)[0]
        });
      }

      if (formVal.productTypeId) {
        this.form.patchValue({
          productType: this.projectTypeList.filter(p => p.id === formVal?.productTypeId)[0]
        });
      }

      this.productTypeId = formVal.productTypeId;
      this.isoRtoId = formVal.isoRtoId;

      this.map.flyTo({
        center: {
          lat: formVal.latitude,
          lng: formVal.longitude
        }
      });

      new mapboxgl.Marker().setLngLat([formVal.longitude, formVal.latitude]).addTo(this.map);

      this.httpService
        .getAny(
          `${environment.mapBox.api}/mapbox.places/${formVal.longitude},${formVal.latitude}.json?access_token=${environment.mapBox.accessToken}`
        )
        .subscribe((res: any) => {
          (document.getElementsByClassName('mapboxgl-ctrl-geocoder--input')[0] as any).value = res.features.find(
            f => f.id.includes('address') || f.id.includes('place')
          ).place_name;
        });

      if (formVal.valuesToOfftakers) {
        formVal.valuesToOfftakers.forEach(vto => {
          this.offtakerValueList.map(v => {
            if (v.id === vto.id) {
              v.selected = true;
            }
          });
        });
      }
    }
  }

  private setupMap() {
    this.map = new mapboxgl.Map({
      accessToken: environment.mapBox.accessToken,
      container: 'map',
      style: environment.mapBox.styles,
      zoom: 12,
      center: [-96, 37.8]
    });

    this.map.addControl(new mapboxgl.NavigationControl());
    this.geocoder = new MapboxGeocoder({
      accessToken: environment.mapBox.accessToken,
      mapboxgl: mapboxgl,
      marker: true
    });

    this.map.addControl(this.geocoder, 'top-left');
    this.geocoder.on('result', e => {
      this.form.patchValue({
        longitude: e.result.geometry.coordinates[0],
        latitude: e.result.geometry.coordinates[1]
      });
    });
  }

  changeStep(step: number): void {
    this.form.controls['latitude'].addValidators(Validators.required);
    this.form.controls['longitude'].addValidators(Validators.required);
    this.form.controls['productType'].addValidators(Validators.required);
    this.form.controls['commercialOperationDate'].addValidators(Validators.required);
    this.form.controls['valuesToOfftakers'].addValidators(Validators.required);
    this.form.controls['ppaTermYearsLength'].addValidators(Validators.required);
    this.form.controls['totalProjectNameplateMWACCapacity'].addValidators(Validators.required);
    this.form.controls['totalProjectExpectedAnnualMWhProductionP50'].addValidators(Validators.required);
    this.form.controls['minimumOfftakeMWhVolumeRequired'].addValidators(Validators.required);
    this.updateFormValidity();

    this.formSubmitted = true;
    if (this.form.invalid) return;

    this.updateProjectDetailsForm();
    this.addProjectService.changeStep(step);
  }

  updateProjectDetailsForm(): void {
    const formForPayload = { ...this.form.value };
    delete formForPayload.isoRto;
    delete formForPayload.productType;

    this.addProjectService.updateProjectTypeData(formForPayload);
    this.addProjectService.updateProjectTypeData({
      isoRtoId: this.isoRtoId,
      productTypeId: this.productTypeId
    });
  }

  goBack(step: number): void {
    this.updateProjectDetailsForm();
    this.addProjectService.changeStep(step);
  }

  chooseValuesToOfftakers(val: TagInterface, unselect: boolean) {
    if (unselect) {
      const updatedVal: number[] = [...this.form.value.valuesToOfftakers].filter(o => o.id !== val.id);

      this.form.patchValue({
        valuesToOfftakers: [...updatedVal]
      });
    } else {
      this.form.patchValue({
        valuesToOfftakers: this.form.value?.valuesToOfftakers ? [...this.form.value.valuesToOfftakers, val] : [val]
      });
    }
  }

  setControlValue(dropdownVal: TagInterface | CountryInterface, controlName: string) {
    this[controlName] = dropdownVal.id;
  }

  saveDraft(): void {
    this.form.controls['latitude'].removeValidators(Validators.required);
    this.form.controls['longitude'].removeValidators(Validators.required);
    this.form.controls['productType'].removeValidators(Validators.required);
    this.form.controls['commercialOperationDate'].removeValidators(Validators.required);
    this.form.controls['valuesToOfftakers'].removeValidators(Validators.required);
    this.form.controls['ppaTermYearsLength'].removeValidators(Validators.required);
    this.form.controls['totalProjectNameplateMWACCapacity'].removeValidators(Validators.required);
    this.form.controls['totalProjectExpectedAnnualMWhProductionP50'].removeValidators(Validators.required);
    this.form.controls['minimumOfftakeMWhVolumeRequired'].removeValidators(Validators.required);
    this.formSubmitted = true;
    this.updateFormValidity();

    if (this.form.invalid) return;

    this.addProjectService.updateProjectTypeData(this.form.value);
    this.updateProjectDetailsForm();
    this.showDraftModal = true;
  }

  updateFormValidity(): void {
    this.form.controls['latitude'].updateValueAndValidity();
    this.form.controls['longitude'].updateValueAndValidity();
    this.form.controls['productType'].updateValueAndValidity();
    this.form.controls['commercialOperationDate'].updateValueAndValidity();
    this.form.controls['valuesToOfftakers'].updateValueAndValidity();
    this.form.controls['ppaTermYearsLength'].updateValueAndValidity();
    this.form.controls['totalProjectNameplateMWACCapacity'].updateValueAndValidity();
    this.form.controls['totalProjectExpectedAnnualMWhProductionP50'].updateValueAndValidity();
    this.form.controls['minimumOfftakeMWhVolumeRequired'].updateValueAndValidity();
  }
}
