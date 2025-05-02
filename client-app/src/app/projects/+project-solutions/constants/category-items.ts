import { ResourceTypeEnum } from 'src/app/core/enums/resource-type.enum';
import { CategoryInterface } from '../interfaces/category.interface';

export const categoryItems: CategoryInterface[] = [
  {
    slug: 'community-solar',
    titleText: 'Community Solar',
    descriptionText:
      'A policy-enabled option for purchasing locally generated solar power; typically only available in the US and in certain states and utility territories.',
    categoryTags: [
      {
        tagTitle: 'Link to Energy Sage article?',
        slug: 'energy-sage-article',
        tagType: ResourceTypeEnum.WebsiteLink
      },
      {
        tagTitle: 'Video Interview with Nextera on Community solar',
        slug: '',
        tagType: ResourceTypeEnum.Video
      },
      {
        tagTitle: 'What is Distributed Generation?',
        slug: 'what-is-distributed-generation',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'battery-storage',
    titleText: 'Battery Storage',
    descriptionText:
      'Battery energy storage systems (BESS). Either installed “stand alone” for the purpose of demand charge management, or in combination with solar PV or other onsite generation.',
    categoryTags: [
      {
        tagTitle: 'Battery storage video interview with SE ops',
        slug: 'battery-storage-video-with-se-ops',
        tagType: ResourceTypeEnum.Video
      },
      {
        tagTitle: 'Interview with solution provider (Alight - AC)',
        slug: 'interview-with-solution-provider-alight-ac',
        tagType: ResourceTypeEnum.Video
      },
      {
        tagTitle: 'Energy Storage | NEO 101 Series',
        slug: 'energy-storage-neo-101-series',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'ev-charging-fleet-electrification',
    titleText: 'EV Charging & Fleet Electrification',
    descriptionText:
      'The installation of electric vehicle charging stations, infrastructure, and often software, whether for building occupants and employees or for fleets.',
    categoryTags: [
      {
        tagTitle: 'Managing the Energy Transformation: The Disruptive Power of Fleet Electrification',
        slug: 'managing-the-energy-transformation-the-disruptive-power-of-fleet-electrification',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'The Decarbonization Challenge, Part 1: Closing the Ambition to Action Gap',
        slug: 'the-decarbonization-challenge-part-1-closing-the-ambition-to-action-gap',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'The Decarbonization Challenge, Part 2: Getting it Done',
        slug: 'the-decarbonization-challenge-part-2-getting-it-don',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'fuel-cells',
    titleText: 'Fuel Cells',
    descriptionText:
      'Technology that produces baseload, low carbon electricity. Typically installed onsite (“behind the meter”), fuel cells can be powered by natural gas, renewable biogas, or hydrogen.',
    categoryTags: [
      {
        tagTitle: 'Interview with Andrew P?',
        slug: 'interview-with-andrew-p',
        tagType: ResourceTypeEnum.Video
      },
      {
        tagTitle: 'Bloom article',
        slug: 'bloom-article',
        tagType: ResourceTypeEnum.PDF
      },

      {
        tagTitle: 'Bloom article (reduced)',
        slug: '',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'emerging-technologies',
    titleText: 'Emerging Technologies',
    descriptionText:
      'A catch-all for various emerging technologies to reduce carbon and/or produce low-carbon or zero-carbon electricity.',
    categoryTags: [
      {
        tagTitle: 'Emerging technology SE ops video interview',
        slug: 'emerging-technology-se-ops-video-interview',
        tagType: ResourceTypeEnum.Video
      }
    ]
  } as CategoryInterface,
  {
    slug: 'onsite-solar',
    titleText: 'Onsite Solar',
    descriptionText:
      'Solar photovoltaic (PV) installed onsite, whether on the rooftop, on carports, or ground-mounted.',
    categoryTags: [
      {
        tagTitle: 'DER Map',
        tagType: ResourceTypeEnum.QlikApplication
      },
      {
        tagTitle: 'What is Distributed Generation?',
        slug: 'what-is-distributed-generation-2',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'FAQ for Considering Onsite Solar',
        slug: 'onsite-solar-faq',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'Onsite solar request tool',
        tagType: ResourceTypeEnum.NativeTool
      }
    ]
  } as CategoryInterface,
  {
    slug: 'carbon-offset-purchasing',
    titleText: 'Carbon Offset Purchasing',
    descriptionText:
      'Also called Verified Emission Reductions (VERs), carbon offsets (or credits) represent verified projects or actions that address one metric ton of carbon emissions.',
    categoryTags: [
      {
        tagTitle: 'Moving Organizations to Carbon Neutrality: The Role of Carbon Offsets',
        slug: 'moving-to-carbon-neutrality-the-role-of-carbon-offsets',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'eac-purchasing',
    titleText: 'EAC Purchasing',
    descriptionText:
      'Energy attribute certificates (EACs) is a generic term for various renewable credit schemes available in different geographies, such as RECs, I-RECs, TIGRs, GOs, REGOs, LGCs, and others. One EAC can be purchased to address one MWh worth of purchased electricity.',
    categoryTags: [
      {
        tagTitle: 'The Role of RECs and Additionality',
        slug: 'the-role-of-recs-and-additionality',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'Understanding Renewable Energy Certificates in Europe',
        slug: 'understanding-renewable-energy-certificates-in-europe',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'efficiency-audits-consulting',
    titleText: 'Efficiency Audits & Consulting',
    descriptionText:
      'A site-level audit which identifies potential process and equipment improvements to reduce electricity consumption.',
    categoryTags: [
      {
        tagTitle: 'What is an Efficiency Audit',
        slug: 'what-is-an-efficiency-audit',
        tagType: ResourceTypeEnum.Video
      },
      {
        tagTitle: 'Energy Efficiency 101',
        slug: 'energy-efficiency-101',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'renewable-retail-electricity',
    titleText: 'Renewable Retail Electricity',
    descriptionText:
      'The purchase of renewable electricity from a retail service provider under various contract structures.',
    categoryTags: [
      {
        tagTitle: 'Retail renewable pdf from slides',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'Video interview with SE sourcing lead on renewable retail',
        slug: 'video-interview-se-sourcing-lead-on-renewable-retail',
        tagType: ResourceTypeEnum.Video
      },
      {
        tagTitle: 'Zeigo piece on retail PPAs',
        slug: 'zeigo-piece-on-retail-ppas',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'utility-green-tariff',
    titleText: 'Utility Green Tariff',
    descriptionText: 'The purchase of renewable electricity from a regulated utility under various structures.',
    categoryTags: [
      {
        tagTitle: 'Utility Green Tarriff Overview',
        slug: 'utility-green-tariff-overview',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'Video interview with SE reg markets on green tariffs',
        slug: 'video-interview-with-se-reg-markets-on-green-tariffs',
        tagType: ResourceTypeEnum.Video
      }
    ]
  } as CategoryInterface,
  {
    slug: 'offsite-power-purchase-agreement',
    titleText: 'Offsite Power Purchase Agreement',
    descriptionText:
      'A “contract for differences,” where the purchaser (“offtaker”) agrees to buy the power, and typically the EACs, from a utility-scale renewable power generation asset at an agreed-upon price. The power is then sold into the grid and the offtaker receives the difference in price, whether positive or negative.',
    categoryTags: [
      {
        tagTitle: 'NEO Curriculum: Power Purchase Agreements (PPAs) 101',
        slug: 'neo-curriculum-power-purchase-agreements-ppas-101-2',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'NEO Curriculum: VPPA 201 for Accounting',
        slug: 'neo-curriculum-vppa-201-for-accounting',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'NEO Curriculum: VPPA 201 for Finance',
        slug: 'neo-curriculum-vppa-201-for-finance',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'Strategy Calculator',
        tagType: ResourceTypeEnum.QlikApplication
      }
    ]
  } as CategoryInterface,
  {
    slug: 'aggregated-ppas',
    titleText: 'Aggregated PPAs',
    descriptionText:
      'An offsite power purchase agreement where multiple offtakers collect their electrical load to contract as a single entity.',
    categoryTags: [
      {
        tagTitle: 'Corporate PPAs: The Collaborative Model',
        slug: 'corporate-ppas-the-collaborative-model',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'Joining the Club: Collaborative Offsite PPA Structures for Renewable Energy Buyers',
        slug: 'joining-the-club-collaborative-offsite-ppa-structures-for-renewable-energy-buyers',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'NEO Curriculum: Power Purchase Agreements (PPAs) 101',
        slug: 'neo-curriculum-power-purchase-agreements-ppas-101',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface,
  {
    slug: 'efficiency-equipment-measures',
    titleText: 'Efficiency Equipment Measures',
    descriptionText:
      'Retrofit projects aimed at improving the energy efficiency of a facility. Examples include lighting, HVAC, and building envelopes.',
    categoryTags: [
      {
        tagTitle: 'Interview with Mike Susa on equipment measures',
        slug: 'interview-with-mike-susa-on-equipment-measures',
        tagType: ResourceTypeEnum.Video
      },
      {
        tagTitle: 'Energy Efficiency 101',
        slug: 'energy-efficiency-101',
        tagType: ResourceTypeEnum.PDF
      },
      {
        tagTitle: 'Recovering Heat from Exhaust Air',
        slug: 'recovering-heat-from-exhaust-air',
        tagType: ResourceTypeEnum.PDF
      }
    ]
  } as CategoryInterface
];
