using NotTahminApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NotTahminApp
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void SignUp(Student s);
        [OperationContract]
        StudentDTO Login(Student s);
        [OperationContract]
        bool ChangePassword(Student s);
        [OperationContract]
        string TryChangePassword(Student s);
        [OperationContract]
        void InsertLesson(Lesson ls);
        [OperationContract]
        List<LessonDTO> GetAllLesson(int id);
        [OperationContract]
        void UpdateLesson(Lesson ls, int uid);
        [OperationContract]
        bool DeleteLesson(int Id, int uid);
        [OperationContract]
        StudentDTO AccessControl(Student s);
        [OperationContract]
        List<StudentDTO> GetAllStudents(int id);
        [OperationContract]
        void DeleteUser(int Id, int uid);
        [OperationContract]
        void UpdateUser(Student user);
    }
}
