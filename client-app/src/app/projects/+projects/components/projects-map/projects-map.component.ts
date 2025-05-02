import { AfterViewInit, Component, Input, OnChanges, SimpleChanges, ViewEncapsulation } from '@angular/core';

import * as mapboxgl from 'mapbox-gl';
import * as GeoJSON from 'geojson';
import { ProjectOffsitePpaInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';
import { environment } from '../../../../../environments/environment';
import { ProjectService } from '../../services/project.service';

@Component({
  selector: 'neo-projects-map',
  templateUrl: './projects-map.component.html',
  styleUrls: ['../../projects.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectsMapComponent implements AfterViewInit, OnChanges {
  @Input() projects: ProjectInterface[];
  map: mapboxgl.Map;
  // Initial cantre to set US view by default
  initialCentre: number[] = [-97.73259773992419, 39.39765732067164];
  initialZoom = 0;
  highlightedId: string = null;
  initalLoad: boolean = true;
  projectFeaturs: GeoJSON.FeatureCollection = {
    type: 'FeatureCollection',
    features: []
  };

  listPopup = new mapboxgl.Popup({
    closeButton: false
  });
  bounds: mapboxgl.LngLatBounds;

  constructor(private readonly projectService: ProjectService) {
    this.projectService.getHoverProject$().subscribe(val => {
      this.popupProjectTitle(val);
    });
  }

  ngAfterViewInit(): void {
    this.setupMap();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['projects']?.currentValue !== changes['projects']?.previousValue) {
      this.setupMap();
    }
  }

  popupProjectTitle(project: ProjectInterface) {
    if (project != null) {
      const offsitePPADetails = project.projectDetails as ProjectOffsitePpaInterface;
      this.listPopup
        .setLngLat([offsitePPADetails?.longitude, offsitePPADetails?.latitude])
        .setHTML(this.getPopMessage(project))
        .addTo(this.map);
      if (this.initalLoad) {
        // To stop movement while hovering over the list
        // this.map.flyTo({
        //   center: [offsitePPADetails?.longitude, offsitePPADetails?.latitude],
        //   zoom: this.initialZoom
        // });
      }
    }
  }

  private setupMap() {
    this.map = new mapboxgl.Map({
      accessToken: environment.mapBox.accessToken,
      container: 'map',
      style: environment.mapBox.styles,
      center: [this.initialCentre[0], this.initialCentre[1]],
      zoom: this.initialZoom
    });

    if (this.projects && this.projects.length > 0) {
      this.bounds = new mapboxgl.LngLatBounds();
      let i = 0;
      this.projectFeaturs.features = [];
      this.projects.forEach(p => {
        const offsitePPADetails = p.projectDetails as ProjectOffsitePpaInterface;
        if (offsitePPADetails.longitude && offsitePPADetails.latitude) {
          var point: GeoJSON.Feature = {
            type: 'Feature',
            geometry: {
              type: 'Point',
              coordinates: [offsitePPADetails.longitude, offsitePPADetails.latitude]
            },
            properties: {
              title: this.getPopMessage(p),
              id: p.id,
              sortId: i++,
              projectTitle: p.title,
              description: p.description,
              coordinates: [offsitePPADetails.longitude, offsitePPADetails.latitude]
            }
          };
          if (point.geometry.type == 'Point' && point.geometry.coordinates != null) {
            this.projectFeaturs.features.push(point);
          }
          this.bounds.extend([offsitePPADetails?.longitude, offsitePPADetails?.latitude]);
        }
      });

      this.map.fitBounds(this.bounds, { maxZoom: 8, padding: 15 });

      this.map.on('load', () => {
        this.map.addSource('project-features', {
          type: 'geojson',
          data: this.projectFeaturs
        });

        this.map.addLayer({
          id: 'points',
          type: 'circle',
          source: 'project-features',
          paint: {
            'circle-color': '#ffffff',
            'circle-radius': 6,
            'circle-stroke-width': 2,
            'circle-stroke-color': '#ffffff'
          }
        });

        this.loadMarker();

        //mouseover event to display the project title at mouse over
        this.map.on('mouseover', 'points', e => {
          // Change the cursor style as a UI indicator.
          this.map.getCanvas().style.cursor = 'pointer';

          // Copy coordinates array.
          const coordinates = e.features[0].properties.coordinates;
          const projectTitle = e.features[0].properties.title;

          // Ensure that if the map is zoomed out such that multiple
          // copies of the feature are visible, the popup appears
          // over the copy being pointed to.
          while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
          }
          // Populate the popup and set its coordinates
          // based on the feature found.
          this.listPopup.setLngLat(JSON.parse(coordinates)).setHTML(projectTitle).addTo(this.map);
        });

        //mouse click event to scroll and highlight the project in list view
        this.map.on('click', 'points', e => {
          this.scroll(document.getElementById('project-' + e.features[0].properties.id));
          if (this.highlightedId != null) {
            var hightlightedElement = document.getElementById(this.highlightedId);
            if (hightlightedElement != null) document.getElementById(this.highlightedId).classList.remove('highlight');
          }
          this.highlightedId = 'project-item-' + e.features[0].properties.id;
          document.getElementById(this.highlightedId).classList.add('highlight');
        });

        //mouseleave event to remove the project title at mouse leave from the point
        this.map.on('mouseleave', 'points', () => {
          this.map.getCanvas().style.cursor = '';
          this.listPopup.remove();
        });

        //moveend event to filter the list and show only projects present in map
        this.map.on('moveend', e => {
          var features = this.map.queryRenderedFeatures(e.point, { layers: ['points'] });
          var projectIds = features.map(a => a.properties.id);
          this.renderZoomProjectList(projectIds);
        });
      });
    }
  }

  scroll(el: HTMLElement) {
    el.parentElement.scrollTop = el.offsetTop - el.parentElement.offsetTop;
  }

  private renderZoomProjectList(projectIds) {
    var result = this.projects.filter(function (project) {
      // filter out (!) items in result2
      return projectIds.some(function (projectid) {
        return project.id == projectid; // assumes unique id
      });
    });
    if (this.initalLoad) this.initalLoad = false;
    this.projectService.setMapProjectList(result);
  }

  private loadMarker() {
    this.projectFeaturs.features.forEach(element => {
      new mapboxgl.Marker().setLngLat(element.properties.coordinates).addTo(this.map);
    });
  }

  private getPopMessage(project: ProjectInterface) {
    return '<p class="project-title">' + project.title + '</p>';
  }
}
