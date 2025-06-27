# Altura Backend Engineer Assessment

## Overview
Deliverable: Develop a fully functional backend application with the specified features.  
**Estimated completion time:** 4 hours.

---

## Submission Guidelines

1. **Repository Setup**  
   - **Create a private repository** using **GitHub** or **Azure DevOps**.  
   - Add the provided boilerplate code as your **first commit**, ensuring that only the boilerplate code is included in this commit.  
   - Grant access to the following users so they can review your submission:  
     - `teshvier@altura.io`  
     - `ayoub@altura.io`
     - `maksim.lizura@altura.io`

2. **Implementation**  
   - Complete the required tasks described in the **Problem Statement** and **Technical Requirements** sections.  
   - Commit your changes incrementally with meaningful commit messages to demonstrate your development process.

3. **Documentation**  
   - Include a `README.md` file that provides:  
     - Clear setup instructions.  
     - Steps to run the application.  
     - Instructions for running tests.

4. **Submission**  
   - Once your implementation is complete, ensure all code is pushed to the repository.  
   - Notify the reviewers (at the provided email addresses) that the repository is ready for review.

---

## Problem Statement

Altura's platform processes bids for large-scale projects. Your task is to develop a backend service to manage project bids. The service should provide functionality to:

1. Create, retrieve, and update bids.
2. Enforce the following constraints:
   - **Unique titles:** Each bid must have a distinct title.
   - **Positive amounts:** The bid amount must be greater than zero.

### Bid Model
A `bid` should consist of the following fields:
- **id** (GUID): Unique identifier.
- **title** (string): Title of the bid (1-100 characters).
- **amount** (decimal): Monetary value of the bid.
- **state** (string): Current status of the bid (`Draft`, `Submitted`, `Approved`).
- **created_at** (datetime): Timestamp of bid creation.
- **updated_at** (datetime): Timestamp of the most recent update.

*You may enhance the model as necessary to meet the requirements.*

---

## Technical Requirements

1. **Tech Stack**
   - **.NET 8**
   - **SQLite** as the database.
   - **xUnit** for testing.

2. **API Endpoints**  
   Implement the following REST API endpoints:
   - **POST /api/bids:** Create a new bid.
   - **GET /api/bids:** Retrieve all bids.
   - **GET /api/bids/{id}:** Retrieve a specific bid by ID.
   - **PUT /api/bids/{id}:** Update an existing bid.

3. **Constraints**
   - Ensure the bid `amount` is always positive.
   - Enforce unique bid `titles`.

4. **Bonus (Optional)**
   - Set up a basic CI/CD pipeline.

---

## Instructions

You will receive a boilerplate project to get started. Follow these steps to complete and submit the assessment:

1. **Clone the Boilerplate Code**  
   - Add the boilerplate code to your private repository as the **first commit**.

2. **Implement Features**  
   - Implement the features described in the **Problem Statement** and **Technical Requirements** sections.
   - Commit changes incrementally to show your development process.

3. **Test and Document**  
   - Write unit tests using **xUnit** to validate the functionality of your application.
   - Document setup and usage instructions in the `README.md`.

4. **Submit for Review**  
   - Push all changes to your private repository.  
   - Notify the reviewers (`teshvier@altura.io` and `jim@altura.io`) that the code is ready for review.

---

## Additional Notes
- If you have any questions during the assessment, feel free to reach out.
- Good luck, and we look forward to reviewing your work!
