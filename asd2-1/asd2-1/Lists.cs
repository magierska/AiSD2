
using System.Collections.Generic;

namespace ASD
{

public interface IList : IEnumerable<int>
    {
    // Je�li element v jest na li�cie to zwraca true
    // Je�li elementu v nie ma na li�cie to zwraca false
    bool Search(int v);

    // Je�li element v jest na li�cie to zwraca false (elementu nie dodaje)
    // Je�li elementu v nie ma na li�cie to dodaje go do listy i zwraca true
    bool Add(int v);

    // Je�li element v jest na li�cie to usuwa go z listy i zwraca true
    // Je�li elementu v nie ma na li�cie to zwraca false
    bool Remove(int v);
    }

//
// dopisa� klas� opisuj�c� element listy
//
public class Elem
    {
        public Elem next;
        public int key;

        public Elem(int k, Elem n = null)
        {
            key = k;
            next = n;
        }
    }

// Zwyk�a lista (nie samoorganizuj�ca si�)
public class SimpleList : IList
    {

        // doda� niezb�dne pola
    public Elem head, tail;
    // Lista si� nie zmienia
    public bool Search(int v)
        {
            if (head == null)
                return false;
            Elem p = head;
            while (p != null)
            {
                if (p.key == v)
                    return true;
                p = p.next;
            }
            return false;  // zmieni�
        }

    // Element jest dodawany na koniec listy
    public bool Add(int v)
        {
            if (head == null)
            {
                head = tail = new Elem(v);
                return true;
            }
            Elem p = head;
            while (p != null)
            {
                if (p.key == v)
                    return false;
                p = p.next;
            }
            tail.next = new Elem(v);
            tail = tail.next;
            return true;  // zmieni�
        }

    // Pozosta�e elementy nie zmieniaj� kolejno�ci
    public bool Remove(int v)
        {
            if (head == null)
                return false;
            Elem pp = null, p = head;
            while (p != null)
            {
                if (p.key == v)
                    break;
                pp = p;
                p = p.next;
            }
            if (p == null)
                return false;
            if (pp == null)
                head = p.next;
            else if (p.next == null)
            {
                pp.next = null;
                tail = pp;
            }
            else
                pp.next = p.next;
            return true;  // zmieni�
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
            // nie wolno modyfikowa� kolekcji
            Elem p = head;
            while (p != null)
            {
                yield return p.key;
                p = p.next;
            }
        }

    // Wymagane przez interfejs IEnumerable<int> - nie zmmienia� (jest gotowe!)
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
        return this.GetEnumerator();
        }

    } // class SimpleList


// Lista z przesnoszeniem elementu, do kt�rego by� dost�p na pocz�tek
public class MoveToFrontList : IList
    {

        // doda� niezb�dne pola
        public Elem head;
    // Znaleziony element jest przenoszony na pocz�tek
    public bool Search(int v)
        {
            if (head == null)
                return false;
            Elem pp = null, p = head;
            while (p != null)
            {
                if (p.key == v)
                {
                    if (p != head)
                    {
                        pp.next = p.next;
                        p.next = head;
                        head = p;
                    }
                    return true;
                }
                pp = p;
                p = p.next;
            }
            return false;  // zmieni�
        }

    // Element jest dodawany na pocz�tku, a je�li ju� by� na li�cie to jest przenoszony na pocz�tek
    public bool Add(int v)
        {
            if (Search(v))
                return false;
            head = new Elem(v, head);
            return true;
        }

    // Pozosta�e elementy nie zmieniaj� kolejno�ci
    public bool Remove(int v)
        {
            if (head == null)
                return false;
            Elem pp = null, p = head;
            while (p != null)
            {
                if (p.key == v)
                {
                    if (p == head)
                        head = p.next;
                    else
                        pp.next = p.next;
                    return true;
                }
                pp = p;
                p = p.next;
            }
            return false;  // zmieni�
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
            // nie wolno modyfikowa� kolekcji
            Elem p = head;
            while (p != null)
            {
                yield return p.key;
                p = p.next;
            }
        }

    // Wymagane przez interfejs IEnumerable<int> - nie zmmienia� (jest gotowe!)
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
        return this.GetEnumerator();
        }

    } // class MoveToFrontList


} // namespace ASD
