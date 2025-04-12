# Three-Tier Web Application Project:
Smart Check-In is a web application developed as part of my final year project aimed at streamlining attendance management for educational institutions. This solution modernizes the manual attendance process by providing real-time tracking, detailed reporting, and an intuitive interface for students, teachers, and admins.

Check it out live here!

https://attendanceappservice.azurewebsites.net/


# **Architecture**
-- Diagram


### Tools

Development Tools
- Visual Sudio 2022
- Microsoft SQL Server Management Studio 2019

Front-end
- HTML, CSS, JavaScript, AJAX, JSON, .NET Framework

Cloud infrastructre
- Azure
  - Azure App Service
  - Azure SQL Database
  - Azure SQL Server
  - Azure Monitoring 
    - Application insights
- AWS (no longer live)
  - AWS Elastic Beanstalk
  - Amazon Relational Database Service (RDS)
  - AWS Cloud Watch

 Project Management / Development Methodology
 - Agile (3 sprints)
 - Trello Boards

Testing
- Regression testing
- JUnit
- JMeter


# Key Features: 
User Roles
- Admin: Full access to manage users, courses, and settings.
- Teacher: Ability to create courses, manage student attendance, and generate reports.
- Student: Can check in to their classes and view attendance history.

Attendance Management:
- Teachers can mark attendance manually.
- Students can perform self-check-ins to mark their presence in class.

Reporting:
- Generates detailed attendance reports, including historical data for teachers, students, and admins.

Cloud Deployment:
- Hosted on Microsoft Azure and AWS infrastructure for scalability, reliability, and easy management.


 

# Project Development
## Development of the project in Visual Studio
![image](https://github.com/user-attachments/assets/7779000a-c681-4244-9895-13ee31e841f1)

## Database Class Diagram
![image](https://github.com/user-attachments/assets/68c9d85f-ccd9-4dd7-a51b-908a95c9f7b7)

## Working Application
![image](https://github.com/user-attachments/assets/ad411a31-212b-4a85-b859-e1fcd53c864f)

(AWS on mobile device)

![image](https://github.com/user-attachments/assets/a26e1172-8ddc-4fb6-9fa5-c761a227e9da)

(Web app responsiveness on iPad Azure deployment)

![image](https://github.com/user-attachments/assets/2b0cdb14-939d-42b1-8f8d-c18ae75ab858)

![image](https://github.com/user-attachments/assets/3bb16994-b982-47b8-8742-917697cf956f)


## JMeter testing

![image](https://github.com/user-attachments/assets/9257e1e2-0eea-40d3-8d16-27cff434bf78)

## JUnit testing

![image](https://github.com/user-attachments/assets/3bd895cb-877f-4a79-a934-fb75a3cef5bb)

## Azure Monitoring
![image](https://github.com/user-attachments/assets/07c9d948-250f-4ae3-b20f-581593d5bd99)

## Azure CloudWatch Monitoring
![image](https://github.com/user-attachments/assets/5192c45e-2783-4657-99c9-3cb8611a0d76)

## User/Usability Testing 
![image](https://github.com/user-attachments/assets/c35ce125-68f1-4b51-9c0e-acd9b7b5c522)


# Project Summary
The project successfully developed an attendance tracking web application, "Smart Check-In", that addresses the need for a streamlined and efficient system to manage student attendance in educational institutions. The application was designed and implemented using an incremental approach, allowing for gradual feature additions and addressing user feedback throughout the development process.
Through the work done in this project, several key findings and contributions can be highlighted:

- Comprehensive User Functionality: The application provides a well-defined set of user roles (admin, teacher, and student). 
- Versatile Cloud Deployment: The project explored the deployment of the attendance tracking system on two leading cloud platforms, AWS and Microsoft Azure.
- Robust Testing and Evaluation: The project employed a rigorous testing strategy.

# Evaluation of the Project Plan and Final Product
- Agile methodology: A significant lesson was the efficacy of breaking down the objectives into smaller needs and logically arranging them throughout the increments. Utilising Trello facilitated the monitoring of backlogs and the identification of specific requirements to be implemented in each incremental phase.
![image](https://github.com/user-attachments/assets/12f3c7c6-db2e-4eff-9f39-220c832dfc67)

- Azure App Service: was used to host the Smart Check-In web application. It provides automatic scaling, high availability, and built-in security features.

- Azure SQL Database: was used to migrate the local database from **SQL Server Management Studio** to a fully-managed relational database service built for the cloud. Handled all the data management for courses, user accounts, attendance logs, and reports.

- Azure Monitoring: was used to keep track of application performance and usage metrics.

- JMeter: was used to simulate multiple users interacting with the application simultaneously to test the system's performance and response time under load.

- NUnit: was used to write and run unit tests for the application's business logic, ensuring individual components like attendance tracking and user authentication functioned correctly.

- Regression testing: was conducted regularly to ensure that new code changes or feature additions did not unintentionally break existing functionality.

# Critical Evaluation / Future Consideration
As my first cloud-based project, developing the Smart Check-In application on Microsoft Azure (and partially experimenting with AWS for comparison) provided me with a solid understanding of the core concepts of cloud infrastructure, deployment models, and the overall Azure ecosystem. This project not only fulfilled its functional objectives—offering robust features such as user authentication, course management, attendance recording, reporting, and administrative control—but also went through rigorous testing cycles, including NUnit, regression, performance, and usability testing. These ensured that the system met the identified requirements and remained reliable, scalable, and user-friendly.

Looking ahead, I am excited to build upon this foundational experience by diving deeper into Azure infrastructure. Although I have not yet fully embraced Infrastructure as Code (IaC) practices in my workflow, it remains a key area for future exploration. As part of my ongoing learning and professional development, I plan to incorporate IaC tools such as Terraform to design and deploy an Azure Landing Zone, and eventually aim to integrate automated pipelines using Azure DevOps. I also foresee implementing automated resource cleanup routines to enhance cost-efficiency and maintain resource hygiene, further deepening my understanding of cloud governance and security.











