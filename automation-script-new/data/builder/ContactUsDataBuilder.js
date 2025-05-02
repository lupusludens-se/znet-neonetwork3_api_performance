import { faker } from "@faker-js/faker";

/**
 * Builder class for constructing API request payloads.
 */
export class ContactUsDataBuilder {
  constructor(data = {}) {
    this.payload = {
      app_name: data.app_name !== undefined ? data.app_name : faker.company.name(),
      description: data.description !== undefined ? data.description : faker.lorem.sentence(),
      version: data.version !== undefined ? data.version : faker.system.semver(),
      author: data.author !== undefined ? data.author : faker.person.fullName(),
      data: {
        firstName: data.firstName !== undefined ? data.firstName : faker.person.firstName(),
        lastName: data.lastName !== undefined ? data.lastName : faker.person.lastName(),
        email: data.email !== undefined ? data.email : faker.internet.email(),
        company: data.company !== undefined ? data.company : faker.company.name(),
        message: data.message !== undefined ? data.message : faker.lorem.sentence(),
        subject: data.subject !== undefined ? data.subject : faker.lorem.words(3),
      },
    };
  }

  withAppName(app_name) {
    if (app_name !== undefined && typeof app_name !== "string") {
      throw new Error(`app_name must be a string, got: ${typeof app_name}`);
    }
    this.payload.app_name = app_name ?? faker.company.name();
    return this;
  }

  withDescription(description) {
    if (description !== undefined && typeof description !== "string") {
      throw new Error(`description must be a string, got: ${typeof description}`);
    }
    this.payload.description = description ?? faker.lorem.sentence();
    return this;
  }

  withVersion(version) {
    if (version !== undefined && typeof version !== "string") {
      throw new Error(`version must be a string, got: ${typeof version}`);
    }
    this.payload.version = version ?? faker.system.semver();
    return this;
  }

  withAuthor(author) {
    if (author !== undefined && typeof author !== "string") {
      throw new Error(`author must be a string, got: ${typeof author}`);
    }
    this.payload.author = author ?? faker.person.fullName();
    return this;
  }

  withFirstName(firstName) {
    if (firstName !== undefined && typeof firstName !== "string") {
      throw new Error(`firstName must be a string, got: ${typeof firstName}`);
    }
    this.payload.data.firstName = firstName ?? faker.person.firstName();
    return this;
  }

  withLastName(lastName) {
    if (lastName !== undefined && typeof lastName !== "string") {
      throw new Error(`lastName must be a string, got: ${typeof lastName}`);
    }
    this.payload.data.lastName = lastName ?? faker.person.lastName();
    return this;
  }

  withEmail(email) {
    if (email !== undefined && typeof email !== "string") {
      throw new Error(`email must be a string, got: ${typeof email}`);
    }
    this.payload.data.email = email ?? faker.internet.email();
    return this;
  }

  withCompany(company) {
    if (company !== undefined && typeof company !== "string") {
      throw new Error(`company must be a string, got: ${typeof company}`);
    }
    this.payload.data.company = company ?? faker.company.name();
    return this;
  }

  withMessage(message) {
    if (message !== undefined && typeof message !== "string") {
      throw new Error(`message must be a string, got: ${typeof message}`);
    }
    this.payload.data.message = message ?? faker.lorem.sentence();
    return this;
  }

  withSubject(subject) {
    if (subject !== undefined && typeof subject !== "string") {
      throw new Error(`subject must be a string, got: ${typeof subject}`);
    }
    this.payload.data.subject = subject ?? faker.lorem.words(3);
    return this;
  }

  /**
   * Builds and returns the full nested payload as a JSON string.
   * @returns {string} - The JSON string representation of the full payload.
   */
  build() {
    const payload = JSON.stringify(this.payload);
    console.log("Generated full payload:", payload);
    return payload;
  }

  /**
   * Builds and returns a flat payload for endpoints like POST /api/contact-us.
   * @returns {string} - The JSON string representation of the flat payload.
   */
  buildFlat() {
    const flatPayload = {
      firstName: this.payload.data.firstName,
      lastName: this.payload.data.lastName,
      email: this.payload.data.email,
      company: this.payload.data.company,
      message: this.payload.data.message,
      subject: this.payload.data.subject,
    };
    const payload = JSON.stringify(flatPayload);
    console.log("Generated flat payload:", JSON.stringify(flatPayload, null, 2));
    return payload;
  }
}
