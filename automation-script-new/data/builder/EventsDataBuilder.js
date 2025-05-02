import { faker } from "@faker-js/faker";

/**
 * Builder class for constructing API request payloads.
 */
export class EventsDataBuilder {
  constructor(data = {}) {
    this.payload = {
      subject: data.subject !== undefined ? data.subject : faker.lorem.words(3),
      description: data.description !== undefined ? data.description : faker.lorem.sentence(),
      highlights: data.highlights !== undefined ? data.highlights : faker.lorem.sentence(),
      isHighlighted: data.isHighlighted !== undefined ? data.isHighlighted : faker.datatype.boolean(),
      location: data.location !== undefined ? data.location : faker.location.city(), // Updated from faker.address.city()
      locationType: data.locationType !== undefined ? data.locationType : faker.number.int({ min: 0, max: 1 }),
      userRegistration: data.userRegistration !== undefined ? data.userRegistration : faker.internet.email(),
      timeZoneId: data.timeZoneId !== undefined ? data.timeZoneId : faker.number.int(),
      recordings: data.recordings !== undefined ? data.recordings : [{ url: faker.internet.url() }],
      links: data.links !== undefined ? data.links : [{ name: faker.commerce.productName(), url: faker.internet.url() }],
      categories: data.categories !== undefined ? data.categories : [{ id: faker.number.int() }],
      occurrences: data.occurrences !== undefined ? data.occurrences : [{ fromDate: faker.date.recent(), toDate: faker.date.future() }],
      moderators: data.moderators !== undefined ? data.moderators : [{ name: faker.person.fullName(), company: faker.company.name(), userId: faker.number.int() }],
      invitedRoles: data.invitedRoles !== undefined ? data.invitedRoles : [{ id: faker.number.int() }],
      invitedRegions: data.invitedRegions !== undefined ? data.invitedRegions : [{ id: faker.number.int() }],
      invitedCategories: data.invitedCategories !== undefined ? data.invitedCategories : [{ id: faker.number.int() }],
      invitedUsers: data.invitedUsers !== undefined ? data.invitedUsers : [{ id: faker.number.int() }],
      inviteType: data.inviteType !== undefined ? data.inviteType : faker.number.int(),
      eventType: data.eventType !== undefined ? data.eventType : faker.number.int({ min: 0, max: 2 }),
      showInPublicSite: data.showInPublicSite !== undefined ? data.showInPublicSite : faker.datatype.boolean(),
      invitedTiers: data.invitedTiers !== undefined ? data.invitedTiers : [{ id: faker.number.int(), companyTypeId: faker.number.int() }],
    };
  }

  // Methods for setting event properties
  withSubject(subject) {
    this.payload.subject = subject ?? faker.lorem.words(3);
    return this;
  }
  withDescription(description) {
    this.payload.description = description ?? faker.lorem.sentence();
    return this;
  }
  withHighlights(highlights) {
    this.payload.highlights = highlights ?? faker.lorem.sentence();
    return this;
  }
  withIsHighlighted(isHighlighted) {
    this.payload.isHighlighted = isHighlighted ?? faker.datatype.boolean();
    return this;
  }
  withLocation(location) {
    this.payload.location = location ?? faker.location.city();
    return this;
  } // Updated from faker.address.city()
  withLocationType(locationType) {
    this.payload.locationType = locationType ?? faker.number.int({ min: 0, max: 1 });
    return this;
  } // Fixed typo: ffaker -> faker
  withUserRegistration(userRegistration) {
    this.payload.userRegistration = userRegistration ?? faker.internet.email();
    return this;
  }
  withTimeZoneId(timeZoneId) {
    this.payload.timeZoneId = timeZoneId ?? faker.number.int();
    return this;
  }
  withRecordings(recordings) {
    this.payload.recordings = recordings ?? [{ url: faker.internet.url() }];
    return this;
  }
  withLinks(links) {
    this.payload.links = links ?? [{ name: faker.commerce.productName(), url: faker.internet.url() }];
    return this;
  }
  withCategories(categories) {
    this.payload.categories = categories ?? [{ id: faker.number.int() }];
    return this;
  }
  withOccurrences(occurrences) {
    this.payload.occurrences = occurrences ?? [{ fromDate: faker.date.recent(), toDate: faker.date.future() }];
    return this;
  }
  withModerators(moderators) {
    this.payload.moderators = moderators ?? [{ name: faker.person.fullName(), company: faker.company.name(), userId: faker.number.int() }];
    return this;
  }
  withInvitedRoles(invitedRoles) {
    this.payload.invitedRoles = invitedRoles ?? [{ id: faker.number.int() }];
    return this;
  }
  withInvitedRegions(invitedRegions) {
    this.payload.invitedRegions = invitedRegions ?? [{ id: faker.number.int() }];
    return this;
  } // Added
  withInvitedCategories(invitedCategories) {
    this.payload.invitedCategories = invitedCategories ?? [{ id: faker.number.int() }];
    return this;
  } // Added
  withInvitedUsers(invitedUsers) {
    this.payload.invitedUsers = invitedUsers ?? [{ id: faker.number.int() }];
    return this;
  } // Added
  withInviteType(inviteType) {
    this.payload.inviteType = inviteType ?? faker.number.int();
    return this;
  } // Added
  withEventType(eventType) {
    this.payload.eventType = eventType ?? faker.number.int({ min: 0, max: 2 });
    return this;
  } // Added
  withShowInPublicSite(showInPublicSite) {
    this.payload.showInPublicSite = showInPublicSite ?? faker.datatype.boolean();
    return this;
  } // Added
  withInvitedTiers(invitedTiers) {
    this.payload.invitedTiers = invitedTiers ?? [{ id: faker.number.int(), companyTypeId: faker.number.int() }];
    return this;
  } // Added

  /**
   * Builds and returns the full payload as an object.
   * @returns {object} - The full payload object.
   */
  build() {
    console.log("Generated full payload:", JSON.stringify(this.payload));
    return this.payload; // Return object instead of JSON string
  }

  /**
   * Builds and returns a flat payload for endpoints like POST /api/contact-us.
   * @returns {string} - The JSON string representation of the flat payload.
   */
  buildFlat() {
    const flatPayload = {
      firstName: this.payload.data?.firstName,
      lastName: this.payload.data?.lastName,
      email: this.payload.data?.email,
      company: this.payload.data?.company,
      message: this.payload.data?.message,
      subject: this.payload.data?.subject,
    };
    const payload = JSON.stringify(flatPayload);
    console.log("Generated flat payload:", JSON.stringify(flatPayload, null, 2));
    return payload;
  }
}
